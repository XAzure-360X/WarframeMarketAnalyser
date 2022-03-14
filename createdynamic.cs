using System;
using System.Text.Json;
using System.IO;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
namespace TestApp
{
    public class Result
    {
        public Payload payload { get; set; }
    }
    public class Payload
    {
        public List<Item> items { get; set; }
    }
    public class PayloadOrders
    {
        public List<Orders> orders { get; set; }
    }
    public class Item
    {
        public string url_name { get; set; }
        public string item_name { get; set; }
        public string id { get; set; }
        public string thumb { get; set; }
    }
    public class Orders
    {
        public string platinum { get; set; }
        public string quantity { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            string jsonString = File.ReadAllText(@"C:/Users\zaid2/desktop/staticresult.txt");
            Result res = JsonSerializer.Deserialize<Result>(jsonString);
            List<Item> itemList = res.payload.items;
            string uri = "https://api.warframe.market/items/";
            List<string> itemUris = new List<string>();
            foreach (Item item in itemList)
            {
                //https://api.warframe.market/items/{url_mame}/orders
                string itemUri = $"{uri}{item.url_name}/{"orders"}";
                itemUris.Add(itemUri);
                //get the json for each itemUri
            }
            Console.WriteLine(itemUris[0]);
            try()
           /* foreach (string itemUri in itemUris)
            {
                string orderspayload = getJson(itemUri);
            }*/
        }
        public static string getJson(string url_items)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.GetAsync(url_items).Result;
            string json = response.Content.ReadAsStringAsync().Result;
            return json;
        }
        public static void WriteToFile(string json, string filepath)
        {
            System.IO.File.WriteAllText(filepath, json);
        }
        /*public static string Deserialize(string jsonString)
        {
            Result jsonOBJ = JsonSerializer.Deserialize<Result>(jsonString);

        }*/
    }
}
