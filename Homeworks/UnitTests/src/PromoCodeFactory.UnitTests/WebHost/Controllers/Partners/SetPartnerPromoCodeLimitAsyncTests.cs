using AutoFixture.AutoMoq;
using AutoFixture;
using Moq;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.WebHost.Controllers;
using System.Collections.Generic;
using System;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using PromoCodeFactory.WebHost.Models;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PromoCodeFactory.UnitTests.WebHost.Controllers.Partners
{
    public class SetPartnerPromoCodeLimitAsyncTests
    {
        private readonly Mock<IRepository<Partner>> _partnersRepositoryMock;
        private readonly PartnersController _partnersController;

        public SetPartnerPromoCodeLimitAsyncTests()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            _partnersRepositoryMock = fixture.Freeze<Mock<IRepository<Partner>>>();
            _partnersController = fixture.Build<PartnersController>().OmitAutoProperties().Create();
        }

        [Fact]
        public async Task SetPartnerPromoCodeLimitAsync_PartnerIsNotFound_ReturnNotFound()
        {
            // Arrange
            var partnerId = Guid.NewGuid();
            _partnersRepositoryMock.Setup(repo => repo.GetByIdAsync(partnerId))
                                                       .ReturnsAsync((Partner)null);

            var request = new SetPartnerPromoCodeLimitRequest { Limit = 50, EndDate = DateTime.Now.AddMonths(1) };

            // Act
            var result = await _partnersController.SetPartnerPromoCodeLimitAsync(partnerId, request);

            // Assert
            result.Should().BeAssignableTo<NotFoundResult>();
        }

        [Fact]
        public async Task SetPartnerPromoCodeLimitAsync_PartnerIsNotActive_ReturnBadRequest()
        {
            // Arrange
            var partner = CreateBasePartner();
            partner.IsActive = false;

            _partnersRepositoryMock.Setup(repo => repo.GetByIdAsync(partner.Id))
                                                          .ReturnsAsync(partner);

            var request = new SetPartnerPromoCodeLimitRequest { Limit = 50, EndDate = DateTime.Now.AddMonths(1) };

            // Act
            var result = await _partnersController.SetPartnerPromoCodeLimitAsync(partner.Id, request);

            // Assert
            result.Should().BeAssignableTo<BadRequestObjectResult>();
        }

        [Fact]
        public async Task SetPartnerPromoCodeLimitAsync_NewLimitSet_NumberOfIssuedPromoCodesReset()
        {
            // Arrange
            var partner = CreateBasePartner();

            _partnersRepositoryMock.Setup(repo => repo.GetByIdAsync(partner.Id))
                .ReturnsAsync(partner);

            var request = new SetPartnerPromoCodeLimitRequest { Limit = 50, EndDate = DateTime.Now.AddMonths(1) };

            // Act
            await _partnersController.SetPartnerPromoCodeLimitAsync(partner.Id, request);

            // Assert
            partner.NumberIssuedPromoCodes.Should().Be(0);
            partner.PartnerLimits.First().CancelDate.Should().NotBeNull();
            _partnersRepositoryMock.Verify(repo => repo.UpdateAsync(partner), Times.Once);
        }

        [Fact]
        public async Task SetPartnerPromoCodeLimitAsync_LimitLessThanOrEqualToZero_ReturnsBadRequest()
        {
            // Arrange
            var partner = CreateBasePartner();

            _partnersRepositoryMock.Setup(repo => repo.GetByIdAsync(partner.Id))
                .ReturnsAsync(partner);

            var request = new SetPartnerPromoCodeLimitRequest { Limit = 0, EndDate = DateTime.Now.AddMonths(1) };

            // Act
            var result = await _partnersController.SetPartnerPromoCodeLimitAsync(partner.Id, request);

            // Assert
            result.Should().BeAssignableTo<BadRequestObjectResult>();
        }

        [Fact]
        public async Task SetPartnerPromoCodeLimitAsync_LimitSet_SavedToDatabase()
        {
            // Arrange
            var partner = CreateBasePartner();
            var initialLimitCount = partner.PartnerLimits.Count;

            _partnersRepositoryMock.Setup(repo => repo.GetByIdAsync(partner.Id))
                                                       .ReturnsAsync(partner);

            var request = new SetPartnerPromoCodeLimitRequest { Limit = 50, EndDate = DateTime.Now.AddMonths(1) };

            // Act
            await _partnersController.SetPartnerPromoCodeLimitAsync(partner.Id, request);

            // Assert
            partner.PartnerLimits.Count.Should().Be(initialLimitCount + 1);
            _partnersRepositoryMock.Verify(repo => repo.UpdateAsync(partner), Times.Once);
        }


        private Partner CreateBasePartner()
        {
            return new Partner
            {
                Id = Guid.NewGuid(),
                Name = "MyTestPartner",
                IsActive = true,
                NumberIssuedPromoCodes = 5,
                PartnerLimits = new List<PartnerPromoCodeLimit>
                 {
                     new PartnerPromoCodeLimit
                     {
                         Id = Guid.NewGuid(),
                         Limit = 10,
                         CreateDate = DateTime.Now.AddDays(-10),
                         EndDate = DateTime.Now.AddDays(10),
                         CancelDate = null
                     }
                 }
            };
        }
    }
}