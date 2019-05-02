using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Xunit;
using JsonConverterHelper;

namespace JsonConverterHelper.Test
{
    public class KeysAsListJsonConverterTest
    {
        public class ClassWithId
        {
            public int Id { get; set; }
        }

        [JsonConverter(typeof(KeysAsListJsonConverter<ClassWithId, KeysAsList>))]
        public class KeysAsList : List<ClassWithId>
        {
        }

        [Fact]
        public void Test_Deserialize()
        {
            var json = @"{ ""1"" : { ""id"": 1 }, 
            ""2"" : { ""id"": 2 }}";

            var result = JsonConvert.DeserializeObject<KeysAsList>(json);

            Assert.Equal(2, result.Count);
            Assert.Equal(1, result[0].Id);
            Assert.Equal(2, result[1].Id);
        }  
        
        [Fact]
        public void Test_Serialize()
        {
            var list = new KeysAsList
            {
                new ClassWithId {
                    Id = 1,
                },
                new ClassWithId {
                    Id = 2,
                }
            };

            var result = JsonConvert.SerializeObject(list);

            Assert.Equal(@"{""1"":{""Id"":1},""2"":{""Id"":2}}", result);
        }

        public class ClassWithoutId
        {
            public int OtherId { get; set; }
        }

        [JsonConverter(typeof(KeysAsListJsonConverter<ClassWithoutId, ClassWithoutIdList>))]
        public class ClassWithoutIdList : List<ClassWithoutId>
        {
        }

        [Fact]
        public void Test_SerializeWithoutId()
        {
            var list = new ClassWithoutIdList
            {
                new ClassWithoutId {
                    OtherId = 1,
                },
                new ClassWithoutId {
                    OtherId = 2,
                }
            };

            var result = JsonConvert.SerializeObject(list);

            Assert.Equal(@"{""IdNotFound"":{""OtherId"":1},""IdNotFound"":{""OtherId"":2}}", result);
        }
    }
}
