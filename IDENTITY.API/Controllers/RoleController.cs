﻿using IDENTITY.API.Attributes;
using IDENTITY.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IDENTITY.API.Controllers
{
    [CustomAuthorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _RoleService;

        private readonly ILogger<RoleController> _logger;
        public RoleController(
            ILogger<RoleController> logger,
             IRoleService userService)
        {
            _logger = logger;
            _RoleService = userService;
        }
        [HttpPost("AssignRole")]
        public async Task<IActionResult> AssignRoleAsync(Guid id, string role)
        {
            try
            {
                await _RoleService.AssignRole(id, role);
                return Ok();
            }
            catch (Exception ms)
            {
                return BadRequest(ms);
            }
        }
        [HttpPost("ReAssignRole")]
        public async Task<IActionResult> ReAssignRoleAsync([FromQuery]Guid Id, [FromQuery] string Role)
        {
            try
            {
                await _RoleService.ReAssignRole(Id, Role);
                return Ok();
            }
            catch (Exception ms)
            {
                return BadRequest(ms);
            }
        }
        [HttpPost("UnAssignRole")]
        public async Task<IActionResult> UnAssignRoleAsync(Guid id, string role)
        {
            try
            {
                await _RoleService.UnAssignRole(id, role);
                return Ok();
            }
            catch (Exception ms)
            {
                return BadRequest(ms);
            }
        }
        [HttpGet("GetRoles")]
        public async Task<IActionResult> GetRolesAsync()
        {
            try
            {
                return Ok(await _RoleService.GetRolesAsync());
            }
            catch (Exception ms)
            {
                return BadRequest(ms);
            }
        }
        [HttpGet("GetRolesForUser")]
        public async Task<IActionResult> GetRolesForUserAsync([FromQuery] Guid id)
        {
            try
            {
                return Ok(await _RoleService.GetRolesForUserAsync(id));
            }
            catch (Exception ms)
            {
                return BadRequest(ms);
            }
        }
    }
}

