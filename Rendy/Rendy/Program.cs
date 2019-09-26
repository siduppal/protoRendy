using AdaptiveCards;
using AdaptiveCards.Rendering.Html;
using CommandLine;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Dynamic;
using System.IO;

namespace Rendy
{
    public class Options
    {
        [Option('d', "data", Required = true, HelpText = "Data to bind the card to.")]
        public string Data { get; set; }

        [Option('c', "card", Required = true, HelpText = "Card to render.")]
        public string Card { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                   .WithParsed<Options>(o =>
                   {
                       var dataContentsJson = File.ReadAllText(o.Data);
                       var cardContentsJson = File.ReadAllText(o.Card);

                       var generatedCardJson = DataBindCard(dataContentsJson, cardContentsJson);
                       var renderedCard = RenderAdaptiveCardHtml(generatedCardJson);

                       var outputFilePath = GenerateHtmlPageWithDemoCard(o.Data, o.Card, renderedCard);

                       Console.WriteLine($"Generated: {outputFilePath}");
                   });
        }

        private static string GenerateHtmlPageWithDemoCard(string dataFilePath, string cardFilePath, string renderedCard)
        {
            // Parse the template:
            var template = Mustachio.Parser.Parse(File.ReadAllText("HtmlOutputTemplate.html"));

            dynamic model = new ExpandoObject();

            model.dataFilePath = dataFilePath;
            model.cardFilePath = cardFilePath;
            model.generatedAdaptiveCard = renderedCard;

            var content = template(model).ToString();

            // Couldn't find a way to inject card HTML so using a placeholder
            content = content.Replace("%%CARDGOESHERE%%", renderedCard);

            var outputFilePath = $"{dataFilePath}.html";

            File.WriteAllText(outputFilePath, content);

            return outputFilePath;
        }

        private static string RenderAdaptiveCardHtml(string adaptiveCardJson)
        {
            var generatedCard = JsonConvert.DeserializeObject<AdaptiveCard>(adaptiveCardJson);

            var renderer = new AdaptiveCardRenderer();

            var renderedCard = renderer.RenderCard(generatedCard);

            foreach (var w in renderedCard.Warnings)
            {
                Console.Error.WriteLine($"[warning] {w.Code}: {w.Message}");
            }

            return renderedCard.Html.ToString();
        }
    
        private static string DataBindCard(string dataContents, string cardBody)
        {
            // Parse the template:
            var template = Mustachio.Parser.Parse(cardBody);

            // Read the model:
            dynamic model = JsonConvert.DeserializeObject<ExpandoObject>(dataContents, new ExpandoObjectConverter());

            // Combine the model with the template to get content:
            var content = template(model);

            return content;
        }
    }
}
