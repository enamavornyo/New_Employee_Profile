using System;
using System.Collections.Generic;
using New_Employee_Profile.Models;
using Microsoft.EntityFrameworkCore;

namespace New_Employee_Profile.Context
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> contextOptions) : base(contextOptions)
        { }


        //
        public DbSet<NewEmployee> NewEmployees { get; set; }
        public DbSet<EmpType> EmpTypes { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Section> Sections { get; set; }


    }
}

