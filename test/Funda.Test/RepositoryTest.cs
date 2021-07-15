using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Funda.Data;
using Funda.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Funda.Test
{
    public class RepositoryTest
    {
        [Fact]
        public async Task GetAll_ShoudReturn_AllEntities()
        {
            // Arrange
            var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options;
            using (var dbContext = new ApplicationDbContext(contextOptions))
            {
                await dbContext.Houses.AddRangeAsync(GetHouseList());
                await dbContext.SaveChangesAsync();
            }


            using (var dbContext = new ApplicationDbContext(contextOptions))
            {
                // Act
                var repository = new Repository<House>(dbContext);
                var allHouses = repository.GetAll();

                // Assert
                Assert.NotNull(allHouses);
                Assert.NotEmpty(allHouses);
                Assert.Equal(6, await allHouses.CountAsync());
            }
        }

        [Fact]
        public async Task Filter_ShoudReturnFilteredEntities()
        {
            // Arrange
            var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options;
            using (var dbContext = new ApplicationDbContext(contextOptions))
            {
                await dbContext.Houses.AddRangeAsync(GetHouseList());
                await dbContext.SaveChangesAsync();
            }

            using (var dbContext = new ApplicationDbContext(contextOptions))
            {
                // Act
                var repository = new Repository<House>(dbContext);
                var allHouses = repository.Filter(h => h.HasTuin);

                // Assert
                Assert.NotNull(allHouses);
                Assert.NotEmpty(allHouses);
                Assert.Equal(3, await allHouses.CountAsync());
            }
        }


        [Fact]
        public async Task AddAsync_ShoudReturnAddAnEntity()
        {
            // Arrange
            var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options;


            using (var dbContext = new ApplicationDbContext(contextOptions))
            {
                await dbContext.Houses.AddRangeAsync(GetHouseList());
                await dbContext.SaveChangesAsync();
            }

            using (var dbContext = new ApplicationDbContext(contextOptions))
            {
                // Act
                var house = GetHouseList().FirstOrDefault(h => h.MakelaarId == 3);
                var repository = new Repository<House>(dbContext);
                var savedHouse = await repository.AddAsync(house);

                // Assert
                Assert.NotNull(savedHouse);
                Assert.Equal(house.Id, savedHouse.Id);
            }
        }


        [Fact]
        public async Task UpdateAsync_ShoudReturnFilteredEntities()
        {
            // Arrange
            var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options;
            using (var dbContext = new ApplicationDbContext(contextOptions))
            {
                await dbContext.Houses.AddRangeAsync(GetHouseList());
                await dbContext.SaveChangesAsync();
            }

            using (var dbContext = new ApplicationDbContext(contextOptions))
            {
                // Act                
                var repository = new Repository<House>(dbContext);
                var house = repository.Filter(h => h.MakelaarId == 3).FirstOrDefault();
                house.HasTuin = true;
                var updatedHouse = await repository.UpdateAsync(house);

                // Assert
                Assert.NotNull(updatedHouse);
                Assert.Equal(house.HasTuin, updatedHouse.HasTuin);
            }
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