using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ZeroX.API.Mappers;
using ZeroX.API.ViewModels;
using ZeroX.DB.Mappers;
using ZeroX.DB.Models;
using ZeroX.Infrastructure.Interfaces;
using ZeroX.Infrastructure.SignalR.Hubs;

namespace ZeroX.API.Controllers;

[Route("account")]
public class AccountController : Controller
{
    private readonly ILogger<AccountController> _logger;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;
    private readonly IJwtGenerator _generator;
    private readonly IHubContext<GameHub> _context; 
    
    public AccountController(ILogger<AccountController> logger,
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager, IJwtGenerator generator, IHubContext<GameHub> context)
    {
        _logger = logger;
        _userManager = userManager;
        _signInManager = signInManager;
        _generator = generator;
        _context = context;
    }

    [HttpGet]
    [Route("rating")]
    public Task<List<RatingViewModel>> Rating()
        => Task.FromResult(_userManager.Users.OrderByDescending(u => u.Rate)
            .Take(10)
            .Select(u => u.IdentityToRating())
            .ToList());

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return Ok("Имеются ошибки/данные не прошли валидацию => вы ввели что-то не то.");
        }

        try
        {
            _logger.LogInformation("User (Email: {Email}) wants to register", model.Email);
            var user = new AppUser {Email = model.Email, UserName = model.Username};
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                _logger.LogInformation("User (Email: {Email}) successfully registered", model.Email);
                user = await _userManager.FindByEmailAsync(model.Email);
                return Ok(user.Id);
            }

            _logger.LogInformation("User (Email: {Email}) has already registered", model.Email);
            foreach (var error in result.Errors)
            {
                _logger.LogError(error.Description);
                ModelState.AddModelError("", error.Description);
            }

            return Ok("Количество ошибок регистрации: " + ModelState.ErrorCount);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while register");
            return BadRequest(model);
        }
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return Ok("Имеются ошибки/данные не прошли валидацию => вы ввели что-то не то.");
        }
        
        try
        {
            _logger.LogInformation("User with email: {Email} wants to login", model.Email);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null)
            {
                ModelState.AddModelError("", "Пользователя с такой почтой не существует");
                return Ok(ModelState[""]);
            }
            

            if (!(await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false)).Succeeded)
            {
                ModelState.AddModelError("", "Неправильный пароль");
                return Ok(ModelState[""]);
            }

            await _signInManager.SignInAsync(user, model.RememberMe);
            return Ok(new UserViewModel
            {
                DisplayName = user.UserName,
                Token = _generator.CreateToken(user.IdentityToDto()),
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while Login");
            return BadRequest(model);
        }
    }
    
    [HttpPost]
    [Route("rate/{userId}")]
    public async Task<IActionResult> Rate(string userId, int value)
    {
        var user = await _userManager.FindByIdAsync(userId);
        user.Rate += value;
        await _userManager.UpdateAsync(user);
        return Ok(user.Rate);
    }
}