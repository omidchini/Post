using System;
using System.Runtime.Serialization;

using AutoMapper;

using NUnit.Framework;

using Post.Application.Common.Mappings;
using Post.Application.Zones.Queries.GetDeliveries;
using Post.Domain.Entities;

namespace Post.Application.UnitTests.Common.Mappings {
    public class MappingTests {
        private readonly IConfigurationProvider _configuration;

        private readonly IMapper _mapper;

        public MappingTests() {
            _configuration = new MapperConfiguration(cfg => { cfg.AddProfile<MappingProfile>(); });

            _mapper = _configuration.CreateMapper();
        }

        [Test]
        public void ShouldHaveValidConfiguration() {
            _configuration.AssertConfigurationIsValid();
        }

        [Test]
        [TestCase(typeof(Zone), typeof(ZoneDto))]
        [TestCase(typeof(Delivery), typeof(DeliveryDto))]
        public void ShouldSupportMappingFromSourceToDestination(Type source, Type destination) {
            var instance = GetInstanceOf(source);

            _mapper.Map(instance, source, destination);
        }

        private object GetInstanceOf(Type type) {
            return type.GetConstructor(Type.EmptyTypes) != null ? Activator.CreateInstance(type) : FormatterServices.GetUninitializedObject(type);

            // Type without parameterless constructor
        }
    }
}