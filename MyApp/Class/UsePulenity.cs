using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pullenti.Ner;
using ServiceStack;

namespace MyApp.Class
{
    public static class UsePulenity
    {
        static UsePulenity()
        {
            Pullenti.Sdk.InitializeAll();
        }

        public static async Task<AnalysisResult> ReturnPullenity(string text)
        {
            // создаём экземпляр процессора со стандартными анализаторами
            Processor processor = ProcessorService.CreateProcessor();
            //processor.Analyzers
            SourceOfAnalysis source = new SourceOfAnalysis(text);
            // запускаем на тексте text
            AnalysisResult result = processor.Process(source);

            //processor.ProcessNext(result);
            //получили выделенные сущности
            //foreach (Referent entity in result.Entities)
            //{
            //    Console.WriteLine("____________________________________________");
            //    Console.WriteLine(entity.ToString());
            //    //Console.WriteLine(entity.InstanceOf.Caption);
            //    Console.WriteLine(entity.TypeName);
            //    Console.WriteLine(entity.Serialize());
            //    //var temp=entity.Slots;
            //    //foreach (Slot element in temp)
            //    //{
            //    //    string jsonString = JsonConvert.SerializeObject(element);
            //    //    Console.WriteLine(jsonString);
            //    //}
            //}

            return result;
        }
    }
}
