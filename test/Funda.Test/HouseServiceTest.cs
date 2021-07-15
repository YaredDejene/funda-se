using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Funda.Data;
using Funda.Models;
using Funda.Repositories;
using Funda.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.Protected;
using Xunit;

namespace Funda.Test
{
    public class HouseServiceTest
    {
        private readonly Mock<IRepository<House>> _houseRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;

        public HouseServiceTest()
        {
            _houseRepositoryMock = new Mock<IRepository<House>>();
            _mapperMock = new Mock<IMapper>();

            // Setup
            _houseRepositoryMock.Setup(x => x.GetAll()).Returns(GetHouseList().AsQueryable());
            _houseRepositoryMock.Setup(x => x.Filter(It.IsAny<Expression<Func<House, bool>>>())).Returns((GetHouseList().Where(w => w.HasTuin)).AsQueryable());
            _houseRepositoryMock.Setup(x => x.AddAsync(It.IsAny<House>())).ReturnsAsync(GetHouseList().FirstOrDefault(w => w.MakelaarId == 3));
            _houseRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<House>())).ReturnsAsync(GetHouseList().FirstOrDefault(w => w.MakelaarId == 3));
            _mapperMock.Setup( x => x.Map<House>(It.IsAny<HouseModel>())).Returns(GetHouseList().FirstOrDefault(w => w.MakelaarId == 3));
        }

        [Fact]
        public void GetMaakelaarWithHighestNumberOfListing_ShoudReturn_Top1Maakelaar()
        {
            // Arrange
            var houseService = new HouseService(_houseRepositoryMock.Object, _mapperMock.Object);

            // Act
            var top1Maakelaar = houseService.GetMaakelaarWithHighestNumberOfListing();

            // Assert
            Assert.NotNull(top1Maakelaar);
            Assert.Equal("Makelaar 1", top1Maakelaar.MakelaarNaam);
        }

        [Fact]
        public void GetMaakelaarWithHighestNumberOfListingWithTuin_ShoudReturn_Top1Maakelaar()
        {
            // Arrange
            var houseService = new HouseService(_houseRepositoryMock.Object, _mapperMock.Object);

            // Act
            var top1Maakelaar = houseService.GetMaakelaarWithHighestNumberOfListing(true);

            // Assert
            Assert.NotNull(top1Maakelaar);
            Assert.Equal("Makelaar 1", top1Maakelaar.MakelaarNaam);
        }

        [Fact]
        public void GetTop10MaakelaarsWithHighestNumberOfListing_ShoudReturn_Top10Maakelaars()
        {
            // Arrange
            var houseService = new HouseService(_houseRepositoryMock.Object, _mapperMock.Object);

            // Act
            var top10Maakelaars = houseService.GetTop10MaakelaarsWithHighestNumberOfListing();

            // Assert
            Assert.NotNull(top10Maakelaars);
            Assert.NotEmpty(top10Maakelaars);
            Assert.Equal(3, top10Maakelaars.Count());
        }

        [Fact]
        public void GetTop10MaakelaarsWithHighestNumberOfListingWithTuin_ShoudReturn_Top10Maakelaars()
        {
            // Arrange
            var houseService = new HouseService(_houseRepositoryMock.Object, _mapperMock.Object);

            // Act
            var top10Maakelaars = houseService.GetTop10MaakelaarsWithHighestNumberOfListing(true);

            // Assert
            Assert.NotNull(top10Maakelaars);
            Assert.NotEmpty(top10Maakelaars);
            Assert.Equal(2, top10Maakelaars.Count());
        }

        [Fact]
        public async Task SaveHouses_ShoudSave_Houses()
        {
            // Arrange
            var houseService = new HouseService(_houseRepositoryMock.Object, _mapperMock.Object);
            var houses = new List<HouseModel>(){
                new HouseModel {Id = System.Guid.NewGuid(), MakelaarId = 3, MakelaarNaam = "Makelaar 3", HasTuin = false}
            };

            // Act
            await houseService.SaveHouses(houses);

            // Assert
            _houseRepositoryMock.Verify(p => p.UpdateAsync(It.IsAny<House>()), Times.Once());
        }

        private IEnumerable<House> GetHouseList()
        {
            var houses = new List<House> {
                new House {Id = System.Guid.NewGuid(), MakelaarId = 1, MakelaarNaam = "Makelaar 1", HasTuin = true},
                new House {Id = System.Guid.NewGuid(), MakelaarId = 1, MakelaarNaam = "Makelaar 1", HasTuin = true},
                new House {Id = System.Guid.NewGuid(), MakelaarId = 1, MakelaarNaam = "Makelaar 1", HasTuin = false},
                new House {Id = System.Guid.NewGuid(), MakelaarId = 2, MakelaarNaam = "Makelaar 2", HasTuin = true},
                new House {Id = System.Guid.NewGuid(), MakelaarId = 2, MakelaarNaam = "Makelaar 2", HasTuin = false},
                new House {Id = System.Guid.NewGuid(), MakelaarId = 3, MakelaarNaam = "Makelaar 3", HasTuin = false}
            };
            return houses;
        }
    }
}