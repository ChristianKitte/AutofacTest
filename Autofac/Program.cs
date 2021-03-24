using System;
using System.Dynamic;

namespace Autofac
{
    class Program
    {
        private static IContainer Container { get; set; }
        
        static void Main()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<PersonRepository>().As<IPersonRepository>();
            builder.RegisterType<PersonParser>().As<IPersonParser>();
            builder.RegisterType<Person>().As<IPerson>();
            Container = builder.Build();

            using (var scope = Container.BeginLifetimeScope())
            {
                var x = scope.Resolve<IPerson>();
                x.GetPerson();
            }
            
        }
    }

    public interface IPerson
    {
        void GetPerson();
    }

    public class Person : IPerson
    {
        private IPersonRepository _personRepository = new PersonRepository();
        private IPersonParser _personParser = new PersonParser();

        public Person(IPersonRepository personRepository, IPersonParser personParser)
        {
            this._personParser = personParser;
            this._personRepository = personRepository;
        }

        public void GetPerson()
        {
            Console.Out.WriteLine(this._personParser.ParsePerson(this._personRepository.getPerson()));
        }
    }


    public interface IPersonRepository
    {
        string getPerson();
    }

    public class PersonRepository : IPersonRepository
    {
        public string getPerson()
        {
            return "Peter Schmidt";
        }
    }

    public interface IPersonParser
    {
        string ParsePerson(string Person);
    }

    public class PersonParser : IPersonParser
    {
        public string ParsePerson(string Person)
        {
            return Person.ToUpper();
        }
    }
}
