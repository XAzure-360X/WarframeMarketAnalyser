using System;
using System.Text.Json;
using System.IO;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Linq;
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

    public class Item
    {
        public string url_name { get; set; }
        public string item_name { get; set; }
        public string id { get; set; }
        public string thumb { get; set; }
    }

    public class PayloadOrders
    {
        public Orders payload { get; set;}
    }

    public class Orders
    {
        public List<Order> orders { get; set; }
    }
    public class Order
    {
        public int quantity { get; set; }
        public int platinum { get; set; }
    }
    public class ItemOutput
    {
        public ItemOutput(string uri,List<Order> orders)
        {
            Orders = orders;
            Uri = uri;
        }
        public List<Order> Orders { get; set; }
        public string Uri { get; set;}
    }
    public class ItemsOutput
    {
        public List<ItemOutput> Items { get; set; }
    }
    public class ItemOrderOutput
    {
        public int platinum { get; set; }
        public int quantity { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            List<ItemOutput> items = new List<ItemOutput>();
            string jsonString = File.ReadAllText(@"./staticcrap/staticresult.txt");
            Result res = JsonSerializer.Deserialize<Result>(jsonString);
            List<Item> itemList = res.payload.items;
            string uri = "https://api.warframe.market/v1/items/";
            List<string> itemUris = new List<string>();
            foreach (Item item in itemList)
            {
                //https://api.warframe.market/items/{url_mame}/orders
                string itemUri = $"{uri}{item.url_name}/{"orders"}";
                itemUris.Add(itemUri);
                //get the json for each itemUri
            }
            
                PayloadOrders orderspayload = JsonSerializer.Deserialize<PayloadOrders>(getJson(itemUris[0]));
                List<Order> orders = orderspayload.payload.orders;
                ItemOutput itemOutput = new ItemOutput(uri, orderspayload.payload.orders);
                items.Add(itemOutput);

            ItemsOutput finalitems = new ItemsOutput
            {
                Items = items,
                
            };
            string finaloutput = JsonSerializer.Serialize(finalitems);
            File.WriteAllText(@"./staticcrap/output.json", finaloutput);
            // {item_url} platinum: 'n', quantity: 'y'

            /* try()
             foreach (string itemUri in itemUris)
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

//lets push on git i have an idea
//nope too many people

