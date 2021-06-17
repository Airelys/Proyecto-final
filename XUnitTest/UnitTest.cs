using Microsoft.EntityFrameworkCore;
using Movie_Plus.Data;
using Movie_Plus.Repository;
using Movie_Plus.Services;
using System;
using System.Collections.Generic;
using Xunit;
using XUnitTest.Attributes;

namespace XUnitTest
{
    [TestCaseOrderer("XUnitTest.Order.PriorityOrderer", "XUnitTest")]
    public class UnitTest
    {
        private static Movie_Local movieLocal;
        private static Movie movie;
        private static Horary horary;
        private static Buy_Ticket buyTicket;
        private static List<Reserved_Seats> reservedSeats;

        private static dynamic options =
                new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "MoviePlusPlusdb")
                .Options;

        [Fact(DisplayName = "Testing Movie Local"), TestPriority(1)]
        public void TestingMovieLocal()
        {
            movieLocal = new Movie_Local()
            {
                Local_Name = "Local Test",
                Rows = 4,
                Columns = 2
            };

            using (var context = new ApplicationDbContext(options))
            {
                var LocalRepository = new Repository<Movie_Local>(context);
                var localService = new MovieLocalService(LocalRepository);

                localService.InsertMovie_Local(movieLocal);
            }

            using (var context = new ApplicationDbContext(options))
            {
                var LocalRepository = new Repository<Movie_Local>(context);
                var localService = new MovieLocalService(LocalRepository);

                var result = localService.GetMovie_Local(movieLocal.Id);

                Assert.Equal(result, movieLocal);
            }

        }


        [Fact(DisplayName = "Testing Movie"), TestPriority(2)]
        public void TestingMovie()
        {
            movie = new Movie()
            {
                Title = "Movie Test",
                Duration = 120,
                KindOfMovie = "KindOfMovie Test",
                Country = "Country Test",
                Actors = "Actor Test",
                Ranking = 0,
                PropagandisticAndEconomics = false

            };

            using (var context = new ApplicationDbContext(options))
            {
                var movieRepository = new Repository<Movie>(context);
                var movieService = new MovieService(movieRepository);

                movieService.InsertMovie(movie);
            }

            using (var context = new ApplicationDbContext(options))
            {
                var movieRepository = new Repository<Movie>(context);
                var movieService = new MovieService(movieRepository);

                var result = movieService.GetMovie(movie.Id);

                Assert.Equal(result, movie);
            }

        }

        [Fact(DisplayName = "Testing Horary"), TestPriority(3)]
        public void TestingHorry()
        {
            horary = new Horary()
            {
                MovieId = movie.Id,
                Movie_LocalId = movieLocal.Id,
                Date = DateTime.Now.Date,
                Time = DateTime.Now,
                Price = 0,
                PriceInPoints = 0,
                PointsForBuying = 0,
                ReservedTickets = 0

            };

            using (var context = new ApplicationDbContext(options))
            {
                var horaryRepository = new Repository<Horary>(context);
                var horaryService = new HoraryService(horaryRepository);

                horaryService.InsertHorary(horary);
            }

            using (var context = new ApplicationDbContext(options))
            {
                var horaryRepository = new Repository<Horary>(context);
                var horaryService = new HoraryService(horaryRepository);

                var result = horaryService.GetHorary(horary.Id);

                Assert.Equal(result, horary);
            }

        }

        [Fact(DisplayName = "Testing Ticket"), TestPriority(4)]
        public void TestingBuyTicket()
        {
            buyTicket = new Buy_Ticket()
            {
                HoraryId = horary.Id,
                Date = DateTime.Now

            };

            using (var context = new ApplicationDbContext(options))
            {
                var buyTicketRepository = new Repository<Buy_Ticket>(context);
                var buyTicketService = new BuyTicketService(buyTicketRepository);

                buyTicketService.InsertBuyTicket(buyTicket);
            }

            using (var context = new ApplicationDbContext(options))
            {
                var buyTicketRepository = new Repository<Buy_Ticket>(context);
                var buyTicketService = new BuyTicketService(buyTicketRepository);

                var result = buyTicketService.Get(buyTicket.Id);

                Assert.Equal(result, buyTicket);
            }

        }

        [Fact(DisplayName = "Testing Reserved Seats"), TestPriority(5)]
        public void TestingReserveSeats()
        {
            bool[,] expectedSeats = new bool[4, 2];
            expectedSeats[0, 1] = expectedSeats[1, 1] = expectedSeats[3, 1] = true;

            reservedSeats = new List<Reserved_Seats>()
            {
                new Reserved_Seats() { Buy_TicketId = buyTicket.Id, Row = 0, Column = 1 },
                new Reserved_Seats() { Buy_TicketId = buyTicket.Id, Row = 1, Column = 1 },
                new Reserved_Seats() { Buy_TicketId = buyTicket.Id, Row = 3, Column = 1 }
            };

            using (var context = new ApplicationDbContext(options))
            {
                var reservedSeatsRepo = new Repository<Reserved_Seats>(context);
                var reservedSeatsService = new ReservedSeatsService(reservedSeatsRepo);

                foreach (var item in reservedSeats)
                    reservedSeatsService.InsertReservedSeats(item);
            }

            using (var context = new ApplicationDbContext(options))
            {
                var buyTicketRepository = new Repository<Buy_Ticket>(context);
                var buyTicketService = new BuyTicketService(buyTicketRepository);

                var reservedSeatsRepo = new Repository<Reserved_Seats>(context);
                var reservedSeatsService = new ReservedSeatsService(reservedSeatsRepo);

                var buyEntranceService = new BuyEntranceService(buyTicketService, reservedSeatsService);

                var result = new bool[4, 2];
                result = buyEntranceService.ShowMovieLocalSeats(result, buyTicket.HoraryId.Value);

                for (int i = 0; i < 4; i++)
                    for (int j = 0; j < 2; j++)
                    {
                        Assert.True(result[i, j] == expectedSeats[i, j]);
                    }
            }
        }
        [Fact(DisplayName = "Delete Testing Data"), TestPriority(6)]
        public void DeleteAllTestingData()
        {
            using (var context = new ApplicationDbContext(options))
            {
                var reservedSeatsRepo = new Repository<Reserved_Seats>(context);
                var reservedSeatsService = new ReservedSeatsService(reservedSeatsRepo);

                var buyTicketRepository = new Repository<Buy_Ticket>(context);
                var buyTicketService = new BuyTicketService(buyTicketRepository);

                var horaryRepository = new Repository<Horary>(context);
                var horaryService = new HoraryService(horaryRepository);

                var movieRepository = new Repository<Movie>(context);
                var movieService = new MovieService(movieRepository);

                var LocalRepository = new Repository<Movie_Local>(context);
                var localService = new MovieLocalService(LocalRepository);

                reservedSeatsService.RemoveReservedSeats(reservedSeats);
                buyTicketService.RemoveBuyTicket(buyTicket);
                horaryService.DeleteHorary(horary);
                movieService.DeleteMovie(movie);
                localService.DeleteMovie_Local(movieLocal);
            }
            using (var context = new ApplicationDbContext(options))
            {
                var reservedSeatsRepo = new Repository<Reserved_Seats>(context);
                var reservedSeatsService = new ReservedSeatsService(reservedSeatsRepo);

                var buyTicketRepository = new Repository<Buy_Ticket>(context);
                var buyTicketService = new BuyTicketService(buyTicketRepository);

                var horaryRepository = new Repository<Horary>(context);
                var horaryService = new HoraryService(horaryRepository);

                var movieRepository = new Repository<Movie>(context);
                var movieService = new MovieService(movieRepository);

                var LocalRepository = new Repository<Movie_Local>(context);
                var localService = new MovieLocalService(LocalRepository);

                var horaryResult = horaryService.GetHorary(horary.Id);
                var movieResult = movieService.GetMovie(movie.Id);
                var localResult = localService.GetMovie_Local(movieLocal.Id);
                var buyTicketResut = buyTicketService.Get(buyTicket.Id);
                var reservedSeat = reservedSeatsService.GetReservedSeats(buyTicket.Id);

                Assert.True(horaryResult == null);
                Assert.True(movieResult == null);
                Assert.True(localResult == null);
                Assert.True(buyTicketResut == null);
                Assert.True(reservedSeat.Count == 0);
            }
        }
    }
}
