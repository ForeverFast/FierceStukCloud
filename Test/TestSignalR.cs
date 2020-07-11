using FierceStukCloud_NetCoreLib.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Test
{
    public class TestSignalR
    {
        public User User { get; set; }

        private void AutorizationMethod()
        {
           


            var client = new RestClient("http://localhost:52828/api/Authentication");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("username", "ForeverFast");
            request.AddHeader("password", "789xxx44XX");
            request.AddHeader("device", "PC");
            IRestResponse response = client.Execute(request);

            int Code = 0;
            if (Int32.TryParse(response.Content, out Code) == true)
            {
                switch (Code)
                {
                    case 151:
                        Console.WriteLine("Неверный пароль");
                        break;
                    case 152:
                        Console.WriteLine("Такого логина не существует");
                        break;
                }
            }

            User = JsonSerializer.Deserialize<User>(response.Content);

            if (User != null)
            {
                Console.WriteLine("Авторизован");
            }
            else
            {
                Console.WriteLine("Ошибка входа");
            }
        }




    }
}
