using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AdminController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        public AdminController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("users-with-roles")]
        public async  Task<ActionResult> GetUsersWithRoles()
        {
            var users = await _userManager.Users
            .Include(r=>r.UserRoles) //sử dụng eager loading để tải các UserRoles và Role liên quan đến mỗi User. 
            .ThenInclude(r=>r.Role)//Điều này giúp tránh N+1 problem, một vấn đề hiệu suất phổ biến khi làm việc với các mối quan hệ trong cơ sở dữ liệu.
            .OrderBy(u=>u.UserName)
            .Select(u=>new{
                u.Id,
                Username = u.UserName,
                Roles = u.UserRoles.Select(r=>r.Role.Name).ToList()
            })
            .ToListAsync();
            return Ok(users);
        }

        [HttpPost("edit-roles/{username}")]
        public async Task<ActionResult> EditRoles(string username,[FromQuery] string roles)
        {
            var selectedRoles = roles.Split(",").ToArray();
            var user = await _userManager.FindByNameAsync(username);

            if(user == null) return NotFound("Could not find user");

            var userRoles = await _userManager.GetRolesAsync(user);

            var result = await _userManager.AddToRolesAsync(user,selectedRoles.Except(userRoles));
            if(!result.Succeeded) return BadRequest("Failed to add to roles");

            result = await _userManager.RemoveFromRolesAsync(user,userRoles.Except(selectedRoles));
            if(!result.Succeeded) return BadRequest("False to remove from Roles");
            return Ok(await _userManager.GetRolesAsync(user));
        }

        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpGet("Photos-to-moderate")]
        public ActionResult GetPhotoForMorderate()
        {
            return Ok("Admins or Moderators can see this");
        }
    }
}