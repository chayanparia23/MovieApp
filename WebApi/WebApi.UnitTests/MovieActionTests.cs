using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Business.Actions;
using WebApi.Business.Profiles;
using WebApi.DataAccess.Interfaces;
using WebApi.Models.Entities;
using WebApi.Models.Models;

namespace WebApi.UnitTests
{
    [TestClass]
    public class MovieActionTests
    {
        private Mock<IMovieRepository> _movieRepositoryMock;
        private IMapper _mapper;

        private IEnumerable<Movie> movies; 
        private MovieAction action;


        [TestInitialize]
        public void SetUp()
        {
            _mapper = MoviesProfile.MapAllConfiguration().CreateMapper();
            _movieRepositoryMock = new Mock<IMovieRepository>(MockBehavior.Default);
            action = new MovieAction(_movieRepositoryMock.Object);
            SetUpMockData();
            _movieRepositoryMock.Setup(x => x.GetMovies()).Returns(Task.FromResult(movies));
        }

        [TestMethod]
        public void GetMovies_IfNoSearchText_ReturnAllMovies()
        {
            //Arrange
            UserParamsDto userParamsDto = new UserParamsDto()
            {
                PageNumber = 1,
                PageSize = 5,
                SearchText = string.Empty,
                SearchTextType = string.Empty
            };

            //Act
            var result = action.GetMovies(userParamsDto).GetAwaiter().GetResult();

            //Assert
            Assert.IsTrue(result.Count == 5);
        }

        [TestMethod]
        public void GetMovies_IfNoSearchText_ReturnAllMoviesWithLatestYear()
        {
            //Arrange
            UserParamsDto userParamsDto = new UserParamsDto()
            {
                PageNumber = 1,
                PageSize = 5,
                SearchText = string.Empty,
                SearchTextType = string.Empty
            };

            //Act
            var result = action.GetMovies(userParamsDto).GetAwaiter().GetResult();

            //Assert
            Assert.AreEqual(result[0].Year, 2021);
        }

        [TestMethod]
        public void GetMovies_IfSearchWithTitle_ReturnMoviesWithMatchingSearchedTitledData()
        {
            //Arrange
            UserParamsDto userParamsDto = new UserParamsDto()
            {
                PageNumber = 1,
                PageSize = 5,
                SearchText = "Movie 1",
                SearchTextType = "Title"
            };

            //Act
            var result = action.GetMovies(userParamsDto).GetAwaiter().GetResult();

            //Assert
            Assert.IsTrue(result.Count == 1);
            Assert.AreEqual(result[0].Title, "Test Movie 1");
        }

        [TestMethod]
        public void GetMovies_IfSearchWithYear_ReturnMoviesWithMatchingSearchedYearData()
        {
            //Arrange
            UserParamsDto userParamsDto = new UserParamsDto()
            {
                PageNumber = 1,
                PageSize = 5,
                SearchText = "2018",
                SearchTextType = "Year"
            };

            //Act
            var result = action.GetMovies(userParamsDto).GetAwaiter().GetResult();

            //Assert
            Assert.IsTrue(result.Count == 1);
            Assert.AreEqual(result[0].Year, 2018);
        }

        [TestMethod]
        public void GetMovies_IfSearchWithPlot_ReturnMoviesWithMatchingSearchedPlotData()
        {
            //Arrange
            UserParamsDto userParamsDto = new UserParamsDto()
            {
                PageNumber = 1,
                PageSize = 5,
                SearchText = "Movie 2",
                SearchTextType = "Plot"
            };
            

            //Act
            var result = action.GetMovies(userParamsDto).GetAwaiter().GetResult();

            //Assert
            Assert.IsTrue(result.Count == 1);
            Assert.AreEqual(result[0].Year, 2020);
            Assert.AreEqual(result[0].Plot, "Test Plot For Movie 2");
        }

        [TestMethod]
        public void GetMovies_IfSearchWithGenre_ReturnMoviesWithMatchingSearchedGenreData()
        {
            //Arrange
            UserParamsDto userParamsDto = new UserParamsDto()
            {
                PageNumber = 1,
                PageSize = 5,
                SearchText = "Comedy",
                SearchTextType = "Genre"
            };


            //Act
            var result = action.GetMovies(userParamsDto).GetAwaiter().GetResult();

            //Assert
            Assert.IsTrue(result.Count == 1);
            Assert.AreEqual(result[0].Year, 2020);
            Assert.AreEqual(result[0].Plot, "Test Plot For Movie 2");
        }

        [TestMethod]
        public void GetMovies_IfSearchWithActor_ReturnMoviesWithMatchingSearchedActorData()
        {
            //Arrange
            UserParamsDto userParamsDto = new UserParamsDto()
            {
                PageNumber = 1,
                PageSize = 5,
                SearchText = "Test Actor 2",
                SearchTextType = "Actor"
            };


            //Act
            var result = action.GetMovies(userParamsDto).GetAwaiter().GetResult();

            //Assert
            Assert.IsTrue(result.Count == 3);
        }

        [TestMethod]
        public void GetMovies_IfSearchWithDirector_ReturnMoviesWithMatchingSearchedDirectorData()
        {
            //Arrange
            UserParamsDto userParamsDto = new UserParamsDto()
            {
                PageNumber = 1,
                PageSize = 5,
                SearchText = "Test Director 1",
                SearchTextType = "Director"
            };


            //Act
            var result = action.GetMovies(userParamsDto).GetAwaiter().GetResult();

            //Assert
            Assert.IsTrue(result.Count == 1);
            Assert.AreEqual(result[0].Year, 2018);
        }


        [TestMethod]
        public void GetMovies_IfSearchWithText_ButNoMatching_ThenReturnEmpty()
        {
            //Arrange
            UserParamsDto userParamsDto = new UserParamsDto()
            {
                PageNumber = 1,
                PageSize = 5,
                SearchText = "Test Director 3",
                SearchTextType = "Director"
            };


            //Act
            var result = action.GetMovies(userParamsDto).GetAwaiter().GetResult();

            //Assert
            Assert.IsTrue(result.Count == 0);
        }

        [TestMethod]
        public void GetMovies_IfTotalListIsGreaterThanPageSize_ThenReturnPagedList()
        {
            //Arrange
            UserParamsDto userParamsDto = new UserParamsDto()
            {
                PageNumber = 1,
                PageSize = 4,
                SearchText = string.Empty,
                SearchTextType = string.Empty
            };


            //Act
            var result = action.GetMovies(userParamsDto).GetAwaiter().GetResult();

            //Assert
            Assert.IsTrue(result.Count == userParamsDto.PageSize);
            Assert.IsTrue(result.CurrentPage == userParamsDto.PageNumber);
            Assert.IsTrue(result.TotalPages == 2);
            Assert.IsTrue(result.TotalCount == 5);
        }

        [TestMethod]
        public void GetMovies_IfSearchWithPlot_AndAnyPlotdataIsNull_ReturnMoviesWithMatchingSearchedPlotData()
        {
            //Arrange
            UserParamsDto userParamsDto = new UserParamsDto()
            {
                PageNumber = 1,
                PageSize = 4,
                SearchText = "Movie 1",
                SearchTextType = "Plot"
            };

            //Act
            var result = action.GetMovies(userParamsDto).GetAwaiter().GetResult();

            //Assert
            Assert.IsTrue(result.TotalCount == 1);
            Assert.AreEqual(result[0].Year, 2018);
        }


        private void SetUpMockData()
        {
            movies = new List<Movie>()
            {
                new Movie
                {
                    Id = 1,
                    Year = 2018,
                    Title = "Test Movie 1",
                    ReleaseDate = new DateTime(2018, 1, 30),
                    Rating = 8.2,
                    ImageUrl = "TestUrl",
                    Plot = "Test Plot For Movie 1",
                    Rank = 1,
                    RunningTimeSecs = 900000000,
                    Actors = new List<Actor>()
                    {
                        new Actor { Id = 1, Name = "Test Actor 1" },
                        new Actor { Id = 2, Name = "Test Actor 2" }
                    },
                    Directors = new List<Director>()
                    {
                        new Director { Id = 1, Name = "Test Director 1" }
                    },
                    Genres = new List<Genre>()
                    {
                        new Genre { Id = 1, Type = "Action" }
                    }
                },
                new Movie
                {
                    Id = 2,
                    Year = 2020,
                    Title = "Test Movie 2",
                    ReleaseDate = new DateTime(2020, 1, 30),
                    Rating = 8.4,
                    ImageUrl = "TestUrl2",
                    Plot = "Test Plot For Movie 2",
                    Rank = 1,
                    RunningTimeSecs = 900000000,
                    Actors = new List<Actor>()
                    {
                        new Actor { Id = 1, Name = "Test Actor 3" },
                        new Actor { Id = 2, Name = "Test Actor 2" }
                    },
                    Directors = new List<Director>()
                    {
                        new Director { Id = 1, Name = "Test Director 2" }
                    },
                    Genres = new List<Genre>()
                    {
                        new Genre { Id = 1, Type = "Comedy" }
                    }
                },
                new Movie
                {
                    Id = 3,
                    Year = 2021,
                    Title = "Test Movie 3",
                    ReleaseDate = new DateTime(2021, 1, 30),
                    Rating = 8.4,
                    ImageUrl = "TestUrl3",
                    Plot = "Test Plot For Movie 3",
                    Rank = 3,
                    RunningTimeSecs = 900000000,
                    Actors = new List<Actor>()
                    {
                        new Actor { Id = 1, Name = "Test Actor 3" },
                        new Actor { Id = 2, Name = "Test Actor 2" }
                    },
                    Directors = new List<Director>()
                    {
                        new Director { Id = 1, Name = "Test Director 2" }
                    },
                    Genres = new List<Genre>()
                    {
                        new Genre { Id = 1, Type = "Drama" }
                    }
                },
                new Movie
                {
                    Id = 4,
                    Year = 2019,
                    Title = "Test Movie 4",
                    ReleaseDate = new DateTime(2019, 1, 30),
                    Rating = 8.4,
                    ImageUrl = "TestUrl4",
                    Plot = "Test Plot For Movie 4",
                    Rank = 4,
                    RunningTimeSecs = 900000000,
                    Actors = new List<Actor>()
                    {
                        new Actor { Id = 1, Name = "Test Actor 3" },
                        new Actor { Id = 2, Name = "Test Actor 5" }
                    },
                    Directors = new List<Director>()
                    {
                        new Director { Id = 1, Name = "Test Director 2" }
                    },
                    Genres = new List<Genre>()
                    {
                        new Genre { Id = 1, Type = "Melody" }
                    }
                },
                new Movie
                {
                    Id = 5,
                    Year = 2019,
                    Title = "Test Movie 5",
                    ReleaseDate = new DateTime(2019, 1, 30),
                    Rating = 8.4,
                    ImageUrl = "TestUrl5",
                    Plot = string.Empty,
                    Rank = 5,
                    RunningTimeSecs = 900000000,
                    Actors = new List<Actor>()
                    {
                        new Actor { Id = 1, Name = "Test Actor 3" },
                        new Actor { Id = 2, Name = "Test Actor 6" }
                    },
                    Directors = new List<Director>()
                    {
                        new Director { Id = 1, Name = "Test Director 2" }
                    },
                    Genres = new List<Genre>()
                    {
                        new Genre { Id = 1, Type = "Horrer" }
                    }
                }
            };
        }
    }
}
