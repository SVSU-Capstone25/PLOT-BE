using Plot.Data.Models.User;
using Microsoft.EntityFrameworkCore;


namespace Plot.Context
{//!--------------------This class will be added on to as the database and connections gets worked out more--------------------!
    /// <summary>
    /// Filename: PlotContext.cs
    /// Part of Project: PLOT (can rename later)
    /// 
    /// File Purpose:
    /// This file defines the database context for the PLOT application, which
    /// manages database connections and provides access to entity tables.
    ///
    /// Class Purpose:
    /// This class serves as the database context for the PLOT application, 
    /// interacting with the database using Entity Framework Core. 
    /// It defines data sets that map to database tables. 
    /// Allowing for database manipulation though linq queries.
    /// Options are passed in through dependency injection in Program.cs.
    /// 
    /// Dependencies:
    /// User: A model representing a user in the application.
    /// Entity Framework Core: A library for working with databases using .NET.
    /// 
    /// Written by: Michael Polhill
    /// </summary>
    public class PlotContext : DbContext
    {
        // VARIABLES -- VARIABLES -- VARIABLES -- VARIABLES -- VARIABLES ------

        /// Entity table, represents the Users table in the database.
        public DbSet<User> Users { get; set; }


        // METHODS/FUNCTIONS -- METHODS/FUNCTIONS -- METHODS/FUNCTIONS

        /// <summary>
        /// Constructor for PlotContext.
        /// Initializes a new instance of the database context
        //  with the given options.
        /// </summary>
        /// <param name="options"> Database options used to configure the 
        // database connection and the kind of database.;</param>
        public PlotContext(DbContextOptions<PlotContext> options) : base(options)
        {//Options are configured in Program.cs and passed in here.
        }
    }
}