using NUnit.Framework;
using JsonSrcGen;
using System;
using System.Text;

namespace CustomConverterTests
{
    [CustomConverter(typeof(string))]
    public class CustomCaseStringConverter : ICustomConverter<string>
    {
        public void ToJson(IJsonBuilder builder, string target)
        {
            builder.Append("\"");
            builder.Append(target.ToUpper());
            builder.Append("\""); 
        }

        public ReadOnlySpan<char> FromJson(ReadOnlySpan<char> json, ref string value)
        {
            json = json.SkipWhitespace();
            if(json[0] != '\"')
            {
                throw new InvalidJsonException("String should start with a quote", json);
            }
            json = json.Slice(1);

            var upercase = json.ReadToQuote();

            value = upercase.ToString().ToLower();

            return json.Slice(upercase.Length + 1); 
        }

        public ReadOnlySpan<byte> FromJson(ReadOnlySpan<byte> json, ref string value)
        {
            json = json.SkipWhitespace();
            if(json[0] != '\"')
            {
                throw new InvalidJsonException("String should start with a quote", Encoding.UTF8.GetString(json));
            }
            json = json.Slice(1);

            var upercase = json.ReadToQuote();

            value = Encoding.UTF8.GetString(upercase).ToLower();

            return json.Slice(upercase.Length + 1); 
        }
    }

    [Json]
    public class CustomStringClass 
    {
        public string Property{get;set;}
    }

    public class CustomStringConverterTests
    {
        [Test]
        public void ToJson_CorrectJson() 
        {
            //arrange
            var dateTime = DateTime.MinValue;

            //act
            var json = new JsonConverter().ToJson(new CustomStringClass(){Property="upercase"}); 

            //assert
            Assert.That(json.ToString(), Is.EqualTo("{\"Property\":\"UPERCASE\"}"));
        }

        [Test]
        public void FromJson_CorrectCase() 
        {
            //arrange
            var customClass = new CustomStringClass();

            //act
            new JsonConverter().FromJson(customClass, "{\"Property\":\"UPERCASE\"}"); 

            //assert
            Assert.That(customClass.Property, Is.EqualTo("upercase"));
        }

        [Test]
        public void FromJson_UTF8_CorrectCase() 
        {
            //arrange
            var customClass = new CustomStringClass();

            //act
            new JsonConverter().FromJson(customClass, Encoding.UTF8.GetBytes("{\"Property\":\"UPERCASE\"}")); 

            //assert
            Assert.That(customClass.Property, Is.EqualTo("upercase"));
        }
    }
}