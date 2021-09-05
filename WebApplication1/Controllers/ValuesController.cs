using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;


namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        DataObject dataObject = new DataObject();

        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            if (id > 3 || id < 1)
            {
                return BadRequest();
            }

            IValidator CurrentData = dataObject.DataMap[id];
            return Ok(System.IO.File.ReadAllLines(CurrentData.path));
        }


        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody] JsonElement body)
        {
            if (id > 3 || id < 1)
            {
                return BadRequest();
            }

            string text = body.GetProperty("value").ToString();
            IValidator CurrentData = dataObject.DataMap[id];

            if (CurrentData.IsValid(text) == true)
            {
                System.IO.File.WriteAllText(CurrentData.path, text);
            }
            else
            {
                return BadRequest();
            }

            return Ok(text);
        }
    }


    public interface IValidator
    {
        bool IsValid(string text);
        string path { get; }
    }


    public class DataObject
    {
        public Dictionary<int, IValidator> DataMap = new Dictionary<int, IValidator>
        {
            {1, new OnlyRussian()},
            {2, new OnlyEnglish()},
            {3, new OnlyNubers()}
        };
    }


    public class OnlyNubers : IValidator
    {
        public string path = @"C:\Users\kopyl\Desktop\Новый текстовый документ3.txt";

        string IValidator.path => path;


        public bool IsValid(string text)
        {
            Regex regex = new Regex(@"[\d]*");
            MatchCollection match = regex.Matches(text);


            if (text.Length == match[0].Length)
            {
                return true;
            }

            else
            {
                return false;
            }
        }
    }

    public class OnlyRussian : IValidator
    {
        public string path = @"C:\Users\kopyl\Desktop\Новый текстовый документ1.txt";

        string IValidator.path => path;

        public bool IsValid(string text)
        {
            Regex regex = new Regex(@"[а-я]*", RegexOptions.IgnoreCase);
            MatchCollection match = regex.Matches(text);


            if (text.Length == match[0].Length)
            {
                return true;
            }

            else
            {
                return false;
            }
        }
    }

    public class OnlyEnglish : IValidator
    {
        public string path = @"C:\Users\kopyl\Desktop\Новый текстовый документ2.txt";

        string IValidator.path => path;

        public bool IsValid(string text)
        {
            Regex regex = new Regex(@"[a-z]*", RegexOptions.IgnoreCase);
            MatchCollection match = regex.Matches(text);


            if (text.Length == match[0].Length)
            {
                return true;
            }

            else
            {
                return false;
            }
        }
    }
}