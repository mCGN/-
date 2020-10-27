using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SerialSortTool.Controller
{
    [ApiController]
    [Route("/[controller]/[action]")]
    public class HomeController : ControllerBase
    {
        public HomeController(IHubContext<SerialHub> hubContext)
        {
            HubContext = hubContext;
        }

        public IHubContext<SerialHub> HubContext { get; }

        [HttpGet]
        public ActionResult Start([FromQuery]string Name, [FromQuery]int baud = 9600, [FromQuery]int StopBits = 1, [FromQuery] int Parity = 0, [FromQuery]int DataBits = 8)
        {
            new Tool().Start(Name, baud, StopBits, Parity, DataBits);
            return Ok();
        }

        [HttpGet]
        public ActionResult Stop([FromQuery]string Name)
        {
            new Tool().Stop(Name);
            return Ok();
        }

        /*
        public async Task<IActionResult> TestSend()
        {
            await HubContext.Clients.Group("COM1").SendAsync("received", "测试消息");
            return Ok();
        }
        */
    }
}
