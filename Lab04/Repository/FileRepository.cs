using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Text.Json;
using ProgrammingInCSharp.Lab04.Models;
using ProgrammingInCSharp.Lab04.ViewModels;

namespace ProgrammingInCSharp.Lab04.Repository
{
    class FileRepository
    {
        private static readonly string BaseFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ParnakStorage");
        
        public FileRepository()
        {
            if (!Directory.Exists(BaseFolder))
            {
                Directory.CreateDirectory(BaseFolder);
                
                for (int i = 0; i < 50; i++)
                {
                    var birthdate = new DateTime(1960 + i, (i % 12) + 1, (i % 28) + 1);
                    _ = AddOrUpdateAsync(new Person($"TestUser{i}", $"Surname{i}", $"post{i}@gmail.com", birthdate, Person.FindIsAdult(birthdate), Person.FindSunSign(birthdate.Day, birthdate.Month), Person.FindChineseSign(birthdate.Year), Person.FindIsBirthday(birthdate)));
                }
            }
        }

        public async Task AddOrUpdateAsync(Person person)
        {
            var stringObj = JsonSerializer.Serialize(person);

            using (StreamWriter sw = new StreamWriter(Path.Combine(BaseFolder, person.Email), false))
            {
                await sw.WriteAsync(stringObj);
            }

        }

        public async Task<Person> GetAsync(string email)
        {
            string stringObj = null;
            string filePath = Path.Combine(BaseFolder, email);

            if (!File.Exists(filePath))
                return null;
            
            using (var reader = new StreamReader(filePath))
            {
                stringObj = await reader.ReadToEndAsync();
            }

            return JsonSerializer.Deserialize<Person>(stringObj);
        }

        public void Remove(Person person)
        {
            File.Delete(Path.Combine(BaseFolder, person.Email));
        }

        public List<PersonViewModel> GetAll(Action gotoMain)
        {
            var res = new List<PersonViewModel>();
            foreach (var file in Directory.EnumerateFiles(BaseFolder))
            {
                string stringObj = null;

                using (StreamReader sw = new StreamReader(file))
                {
                    stringObj = sw.ReadToEnd();
                }

                res.Add(new PersonViewModel(JsonSerializer.Deserialize<Person>(stringObj), gotoMain));
            }

            return res;
        }

    }
}
