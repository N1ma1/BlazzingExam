﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazzingExam.Core.DTOs;
using BlazzingExam.Core.Server.Security;
using BlazzingExam.Core.Server.ServerServices.Interfaces;

namespace BlazzingExam.WebApps.Server.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }


        /// <summary>
        /// Login a user
        /// </summary>
        /// <param name="remember">Remember me as boolean</param>
        /// <param name="model">Login View Model</param>
        /// <returns></returns>
        /// <response code="200">User is successfully loged in.</response>
        /// <response code="400">Username or password wrong.</response>      
        /// <remarks>
        /// Post:
        ///    {
        ///        "userName": "Your username",
        ///        "password": "Your password",
        ///        "rememberMe": true
        ///    }
        /// </remarks>
        [HttpPost("[action]/{remember}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Login(bool remember, LoginViewModel model)
        {
            var loginUser = await _userService.LoginUserAsync(model);
            if (loginUser == null)
                return BadRequest();

            await HttpContext.LoginAsync(loginUser, model.RememberMe);

            return Ok();
        }

        /// <summary>
        /// Check if is email exists or not
        /// </summary>
        /// <param name="email">Email address</param>
        /// <returns>True or false</returns>
        /// <remarks>
        ///     Get: /IsEmailExists/Example@gmail.com
        /// </remarks>
        [HttpGet("[action]/{email:required}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<bool> IsEmailExists(string email)
        {
            return await _userService.IsExistEmailAsync(email);
        }

        /// <summary>
        /// Check if is username exists or not
        /// </summary>
        /// <param name="userName">Email address</param>
        /// <returns>True or false</returns>
        /// <remarks>
        ///     Get: /IsUserNameExists/MyUniqueUsername
        /// </remarks>
        [HttpGet("[action]/{userName:required}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<bool> IsUserNameExists(string userName)
        {
            return await _userService.IsUserNameExistAsync(userName);
        }

        /// <summary>
        /// Register a new user
        /// </summary>
        /// <param name="model">Register view model</param>
        /// <returns>Is user successfully registered pr not (True or false)</returns>
        [HttpPost("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<bool> Register(RegisterViewModel model)
        {
            return await _userService.RegisterUserAsync(model);
        }
    }
}
