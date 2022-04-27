using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace JsonExampleProject1
{ 
	class Program
	{
		private static readonly JsonSerializerOptions JsonOptionsAllowComments = new JsonSerializerOptions()
		{
			ReadCommentHandling = JsonCommentHandling.Skip,
			AllowTrailingCommas = true,
		};

		public class TestObject
		{
			public DateTime Date { get; set; }
			public int Int { get; set; }
			public string String { get; set; }
			public bool Boolean { get; set; }
			public long Long { get; set; }
			public double Double { get; set; }
			public double[] DoubleArr { get; set; }
			public int[] IntArr { get; set; }
			public string[] StringArr { get; set; }
		}

		private static async Task Main()
		{
			RunTests();

            //SerializeExample();
            await SerializeToFile();
            //DeserizalizeExample();
            //DeserializeWithJsonDocument();
            //SerializeWithOptions();
            //DeserizalizeWithOptions();
        }

		private static void RunTests()
        {
			//// Float tests
			//Debug.WriteLine(DeserializeTestJson(@"{""Double"": 5.0}")); 
			//Debug.WriteLine(DeserializeTestJson(@"{""Double"": Infinity}"));			// Throws
			//Debug.WriteLine(DeserializeTestJson(@"{""Double"": -Infinity}"));			// Throws
			//Debug.WriteLine(DeserializeTestJson(@"{""Double"": NaN}"));				// Throws
			//Debug.WriteLine(DeserializeTestJson(@"{""Double"": ""Infinity""}")); 
			//Debug.WriteLine(DeserializeTestJson(@"{""Double"": ""-Infinity""}")); 
			//Debug.WriteLine(DeserializeTestJson(@"{""Double"": ""NaN""}"));
			//Debug.WriteLine(DeserializeTestJson(@"{""Double"": ""INF""}")); 
			//Debug.WriteLine(DeserializeTestJson(@"{""Double"": ""-INF""}")); 

			//// Trailing commas
			//Debug.WriteLine(DeserializeTestJson(@"{""Double"": 5.0,}"));
			//Debug.WriteLine(DeserializeTestJson(@"{""Double"": 5.0,,}"));				// Throws

			//// Trailing commas in arrays
			//Debug.WriteLine(DeserializeTestJson(@"{""IntArr"": [5, 10, 15,]}"));
			//Debug.WriteLine(DeserializeTestJson(@"{""IntArr"": [5, 10, 15,,]}"));		// Throws

			//// Extra commas between two tokens
			//Debug.WriteLine(DeserializeTestJson(@"{""Double"": 5.0,, ""Int"": 10}")); // Throws
			//Debug.WriteLine(DeserializeTestJson(@"{""IntArr"": [5, 10,, 15]}"));		// Throws

			//         // Comments inside arrays
			//         Debug.WriteLine(DeserializeTestJson(@"{
			//	""StringArr"": [""hi"", //comment
			//	""second line""]
			//	}", JsonOptionsAllowComments));

			//         Debug.WriteLine(DeserializeTestJson(@"{
			//	""StringArr"": [""hi"",
			//	//
			//	""second line""]
			//	}", JsonOptionsAllowComments));

			//         Debug.WriteLine(DeserializeTestJson(@"{
			//	""StringArr"": [""hi"", /*
			//	comment
			//	goes here */
			//	""second line""]
			//	}", JsonOptionsAllowComments));

			//         // Comments outside arrays
			//         // Single-line comments
			//         Debug.WriteLine(DeserializeTestJson(@"{
			//             /* comment */
			//             ""String"": ""42 is the meaning of life""
			//             }", JsonOptionsAllowComments));

			//Debug.WriteLine(DeserializeTestJson(@"{
			//             //comment*//*hi*/
			//             ""String"": ""42 is the meaning of life""
			//             }", JsonOptionsAllowComments));

			//Debug.WriteLine(DeserializeTestJson(@"{
			//             ""String"": // comment
			//             ""42 is the meaning of life""
			//             }", JsonOptionsAllowComments));

			//Debug.WriteLine(DeserializeTestJson(@"{
			//             ""String"": //
			//             ""42 is the meaning of life""
			//             }", JsonOptionsAllowComments));

			//Debug.WriteLine(DeserializeTestJson(@"{
			//             ""String"": ""42 is the meaning of life"" // comment
			//             }", JsonOptionsAllowComments));

			//Debug.WriteLine(DeserializeTestJson(@"{
			//             ""String"": ""42 is the meaning of life"" //* comment */
			//             }", JsonOptionsAllowComments));

			//// Multi-line comments
			//Debug.WriteLine(DeserializeTestJson(@"{
			//             /* comments
			//             take up
			//             multiple lines*/
			//             ""String"": ""42 is the meaning of life""
			//             }", JsonOptionsAllowComments));

			//Debug.WriteLine(DeserializeTestJson(@"{""String"": /* comments
			//             take up
			//             multiple lines*/
			//             ""42 is the meaning of life""
			//             }", JsonOptionsAllowComments));

			//Debug.WriteLine(DeserializeTestJson(@"{""String"": /*


			//             */
			//             ""42 is the meaning of life""
			//             }", JsonOptionsAllowComments));

			//// existing good surrogate pair
			//Debug.WriteLine(DeserializeAndTest("{ \"String\": \"ABC \\ud800\\udc00 DEF\"}", "ABC \ud800\udc00 DEF")); // passes

			//// invalid surrogates (two high back-to-back)
			//Debug.WriteLine(DeserializeAndTest("{ \"String\": \"ABC \\ud800\\ud800 DEF\"}", "ABC \ufffd\ufffd DEF")); // FAILS STJ

			//// invalid surrogates (two high back-to-back)
			//Debug.WriteLine(DeserializeAndTest("{ \"String\": \"ABC \\ud800\\ud800\\u1234 DEF\"}", "ABC \ufffd\ufffd\u1234 DEF")); // FAILS STJ

			//// invalid surrogates (three high back-to-back)                             
			//Debug.WriteLine(DeserializeAndTest("{ \"String\": \"ABC \\ud800\\ud800\\ud800 DEF\"}", "ABC \ufffd\ufffd\ufffd DEF ")); // FAILS STJ

			//// invalid surrogates (high followed by a good surrogate pair)              
			//Debug.WriteLine(DeserializeAndTest("{ \"String\": \"ABC \\ud800\\ud800\\udc00 DEF\"}", "ABC \ufffd\ud800\udc00 DEF ")); // FAILS STJ

			//// invalid high surrogate at end of string
			//Debug.WriteLine(DeserializeAndTest("{ \"String\": \"ABC \\ud800\"}", "ABC \ufffd")); // FAILS STJ

			//// high surrogate not followed by low surrogate
			//Debug.WriteLine(DeserializeAndTest("{ \"String\": \"ABC \\ud800 DEF\"}", "ABC \ufffd DEF ")); // FAILS STJ

			//// low surrogate not preceded by high surrogate
			//Debug.WriteLine(DeserializeAndTest("{ \"String\": \"ABC \\udc00\\ud800 DEF\"}", "ABC \ufffd\ufffd DEF ")); // FAILS STJ

			//// make sure unencoded invalid surrogate characters don't make it through
			//Debug.WriteLine(DeserializeAndTest("{ \"String\": \"\udc00\ud800\ud800\"}", "\ufffd\ufffd\ufffd")); // FAILS STJ

			//Debug.WriteLine(DeserializeAndTest("{ \"String\": \"ABC \\ud800\\b\"}", "ABC \ufffd\b ")); // FAILS STJ
			//Debug.WriteLine(DeserializeAndTest("{ \"String\": \"ABC \\ud800 \"}", "ABC \ufffd ")); // FAILS STJ
			//Debug.WriteLine(DeserializeAndTest("{ \"String\": \"ABC \\b\\ud800\"}", "ABC \b\ufffd")); // FAILS STJ

			//// Test non-standard whitespace
			//Debug.WriteLine(DeserializeTestJson("{ \x00a0\"String\": \"test\"}")); // FAILS STJ
			//Debug.WriteLine(DeserializeTestJson("{ \"String\x00a0\": \"test\"}"));
			//Debug.WriteLine(DeserializeTestJson("{ \"String\"\x00a0: \"test\"}")); // FAILS STJ
			//Debug.WriteLine(DeserializeTestJson("{ \"String\": \x00a0\"test\"}")); // FAILS STJ
			//Debug.WriteLine(DeserializeTestJson("{ \"String\": \"\x00a0test\"}")); 
			//Debug.WriteLine(DeserializeTestJson("{ \"String\": \"test\"\x00a0}")); // FAILS STJ
			//Debug.WriteLine(DeserializeTestJson("{ \"String\": \"test\"}\x00a0")); // FAILS STJ

			//// Test unicode
			//Debug.WriteLine(DeserializeAndTest("{ \"String\": \"You said science was about admitting what we don\u0092t know\" }", "You said science was about admitting what we don\u0092t know"));

			//// Various whitespace and escape characters
			//Debug.WriteLine(DeserializeTestJson("{ \"String\": \0\"I know something you don't know\" }")); // FAILS STJ
			//Debug.WriteLine(DeserializeTestJson("{ \"String\": \a\"I know something you don't know\" }")); // FAILS STJ
			//Debug.WriteLine(DeserializeTestJson("{ \"String\": \b\"I know something you don't know\" }")); // FAILS STJ
			//Debug.WriteLine(DeserializeTestJson("{ \"String\": \f\"I know something you don't know\" }")); // FAILS STJ
			//Debug.WriteLine(DeserializeTestJson("{ \"String\": \n\"I know something you don't know\" }"));
			//Debug.WriteLine(DeserializeTestJson("{ \"String\": \r\"I know something you don't know\" }"));
			//Debug.WriteLine(DeserializeTestJson("{ \"String\": \t\"I know something you don't know\" }"));
			//Debug.WriteLine(DeserializeTestJson("{ \"String\": \v\"I know something you don't know\" }")); // FAILS STJ

		}

		private static bool DeserializeAndTest(string json, string expectedValue, JsonSerializerOptions options = default)
        {
            try
            {
                var obj = JsonSerializer.Deserialize<TestObject>(json, options);

                return obj.String.Equals(expectedValue);
            }
            catch (JsonException e)
            {
                Debug.WriteLine(">>> JsonException: " + e.Message);

                return false;
            }
            catch (System.ArgumentException e)
            {
				Debug.WriteLine(">>> ArgumentException: " + e.Message);

				return false;
			}
        }

        private static bool DeserializeTestJson(string json, JsonSerializerOptions options = default)
        {
			if (options == default)
            {
				options = new JsonSerializerOptions()
                {
					AllowTrailingCommas = true,
					NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals,
                };
            }

			string serializedJson;

            try
            {
                var obj = JsonSerializer.Deserialize<TestObject>(json, options);
                serializedJson = JsonSerializer.Serialize(obj, options);
			}
            catch (JsonException e)
            {
				Debug.WriteLine(e.Message);

				return false;
            }

			return true;
        }
























		private static void SerializeExample()
		{
			var pets = new List<Pet>
			{
				new Pet { Type = "Cat", Name = "MooMoo", Age = 3.4 },
				new Pet { Type = "Squirrel", Name = "Sandy", Age = 7}
			};

			var person = new Person
			{
				Name = "John",
				Age = 34,
				StateOfOrigin = "England",
				Pets = pets
			};

			Console.WriteLine(JsonSerializer.Serialize(person));
			Console.WriteLine(JsonSerializer.Serialize<Person>(person));
		}

		private static async Task SerializeToFile()
		{
			var pets = new List<Pet>
			{
				new Pet { Type = "Cat", Name = "MooMoo", Age = 3.4 },
				new Pet { Type = "Squirrel", Name = "Sandy", Age = 7}
			};

			var person = new Person
			{
				Name = "John",
				Age = 34,
				StateOfOrigin = "England",
				Pets = pets
			};

			var fileName = "Person.json";

			using var stream = File.Create(fileName);
			await JsonSerializer.SerializeAsync(stream, person);
			await stream.DisposeAsync();

			Console.WriteLine(File.ReadAllText(fileName));
		}

		private static void DeserizalizeExample()
		{
			var jsonPerson = @"{""Name"":""John"",
								""Age"":34,
								""StateOfOrigin"":""England"",
								""Pets"":
									[{""Type"":""Cat"",""Name"":""MooMoo"",""Age"":3.4},
									{""Type"":""Squirrel"",""Name"":""Sandy"",""Age"":7}]}";

			var personObject = JsonSerializer.Deserialize<Person>(jsonPerson);

			Console.WriteLine($"Person's name: {personObject.Name}");
			Console.WriteLine($"Person's age: {personObject.Age}");
			Console.WriteLine($"Person's first pet's name: {personObject.Pets.First().Name}");
		}

		private static void DeserializeWithJsonDocument()
		{
			var unknownJsonStructure = @"{""Product name"":""Fork"",
											""Price"": ""300$"",
											""Categories"":
												[{""Area"":""Kitchen"",""Description"":""Cooking Utensil""},
												{""Area"":""Dinning room"",""Description"":""Dinning Utensil""}]}";

			var unknownObject = JsonDocument.Parse(unknownJsonStructure);
			var productName = unknownObject.RootElement.GetProperty("Product name");

			Console.WriteLine($"Product name: {productName}");

			var categories = unknownObject.RootElement.GetProperty("Categories");

			Console.WriteLine("Categories: ");

			foreach (var category in categories.EnumerateArray())
			{
				Console.WriteLine(category.GetProperty("Area"));
			}
		}

		private static void SerializeWithOptions()
		{
			var pets = new List<Pet>
			{
				new Pet { Type = "Cat", Name = "MooMoo", Age = 3.4 },
				new Pet { Type = "Squirrel", Name = "Sandy", Age = 7}
			};

			var person = new Person
			{
				Name = "John",
				Age = 34,
				StateOfOrigin = "England",
				Pets = pets
			};

			//var person2 = new Person
			//{
			//	Name = "John",
			//	Age = null,
			//	StateOfOrigin = "England",
			//	Pets = pets
			//};

			var options = new JsonSerializerOptions
			{
				WriteIndented = true, 
				DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
			};

			Console.WriteLine(JsonSerializer.Serialize(person, options));
			//Console.WriteLine(JsonSerializer.Serialize(person2, options));
		}

		private static void DeserizalizeWithOptions()
		{
			var jsonPerson = @"{""Name"":""John"",
								""Age"":""34"",
								""StateOfOrigin"":""England"",
								""Pets"":
									[{""Type"":""Cat"",""Name"":""MooMoo"",""Age"":""3.4""},
									{""Type"":""Squirrel"",""Name"":""Sandy"",""Age"":7}]}";

			var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);

			var personObject = JsonSerializer.Deserialize<Person>(jsonPerson,  options);

			Console.WriteLine($"Person's age: {personObject.Age}");
			Console.WriteLine($"Person's first pet's name: {personObject.Pets.First().Age}");
		}

	}
}