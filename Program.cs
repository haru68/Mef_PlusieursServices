using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Collections.Generic;

namespace Mef_PlusieursServices
{
    class Program
    {
        static void Main(string[] args)
        {
            Host host = new Host();
            host.Run();
        }
    }

    internal class Host
    {
        [ImportMany(typeof(IWrite))]
        protected IEnumerable<IWrite> _writers = null;

        public void Run()
        {
            var container = new CompositionContainer();
            container.ComposeParts(this, new ConsoleWriter(), new FileWriter());
            foreach(var writer in _writers)
            {
                writer.Write();
            }
        }
    }

    internal interface IWrite
    {
        void Write();
    }

    [Export(typeof(IWrite))]
    internal class ConsoleWriter : IWrite
    {
        public void Write()
        {
            Console.WriteLine(DateTime.Now + " - Hello !");
        }
    }

    [Export(typeof(IWrite))]
    internal class FileWriter : IWrite
    {
        public void Write()
        {
            string filepath = @".\LogFile.txt";
            string message = DateTime.Now + "- Hello!";
            IEnumerable<String> messages = new List<String>() { message};

            File.AppendAllLines(filepath, messages);
        }
    }
}
