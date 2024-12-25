using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.helpers;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{   [ServiceFilter(typeof(LogUserActivity))] //một bộ lọc dịch vụ, được sử dụng để áp dụng logic tùy chỉnh trước và sau
                                            // khi thực hiện một hành động (ví dụ: ghi lại hoạt động của người dùng)
        [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {
        
    }
}