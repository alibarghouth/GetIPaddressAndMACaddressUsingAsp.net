using EmployeeClass.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Management;
using System.Net;
using System.Net.Sockets;

namespace ipandmac.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MACController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public MACController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> getMac()
        {
            //IPAddress
            IPHostEntry iPHostEntry = Dns.GetHostEntry(Dns.GetHostName());
            string ipAddress = Convert.ToString(iPHostEntry.AddressList.FirstOrDefault(address => address.AddressFamily == AddressFamily.InterNetwork));


            //MACAddress

            var mc = new ManagementClass("win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();

            var macAddress = string.Empty;
            foreach (ManagementObject mo in moc)
            {
                if(macAddress == string.Empty)
                {
                    if ((bool)mo["IPEnabled"] == true) macAddress= mo["MacAddress"].ToString();
                }
                mo.Dispose();
            }
            macAddress = macAddress.Replace(":", "-");

            //Add In Database
            var client = new Client();
            client.IpAddress = ipAddress;
            client.MacAddress = macAddress;

            _context.Clients.AddAsync(client);
            _context.SaveChanges();

            return Ok(client);
        }

    }
}
