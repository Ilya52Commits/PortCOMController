using Microsoft.AspNetCore.Mvc;
using System.IO.Ports;

namespace PortCOMController.Controllers
{
    public class SerialPortController : Controller
    {

        //private static SerialPort serialPort = new SerialPort();
        // private static bool disconnecting = false;

        private static SerialPort _serialPort;

        [HttpPost]
        public IActionResult Connect([FromBody] PortSettings settings)
        {
            if (_serialPort == null || !_serialPort.IsOpen)
            {
                _serialPort = new SerialPort(settings.PortName, settings.BaudRate, settings.Parity, settings.DataBits, settings.StopBits)
                {
                    Handshake = settings.Handshake
                };

                try
                {
                    _serialPort.Open();
                    return Json(new { success = true, message = "Подключено к " + settings.PortName });
                }
                catch (Exception ex)
                {
                    // Логирование ошибки
                   // Console.WriteLine("Ошибка подключения: " + ex.Message);
                    return Json(new { success = false, message = "Ошибка подключения: " + ex.Message });
                }
            }

            return Json(new { success = false, message = "Порт уже открыт" });
        }

        [HttpPost]
        public IActionResult Disconnect()
        {
            if (_serialPort != null && _serialPort.IsOpen)
            {
                try
                {
                    _serialPort.Close();
                    return Json(new { success = true, message = "Порт отключен" });
                }
                catch (Exception ex)
                {
                    // Логирование ошибки
                  //  Console.WriteLine("Ошибка отключения: " + ex.Message);
                    return Json(new { success = false, message = "Ошибка отключения: " + ex.Message });
                }
            }

            return Json(new { success = false, message = "Порт уже закрыт" });
        }

    } 
    public class PortSettings
    {
        public string PortName { get; set; }
        public int BaudRate { get; set; }
        public int DataBits { get; set; }
        public Parity Parity { get; set; }
        public StopBits StopBits { get; set; }
        public Handshake Handshake { get; set; }
    }
}

