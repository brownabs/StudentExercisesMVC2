﻿using Microsoft.Extensions.Configuration;
using StudentExercisesMVC2.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace StudentExercisesMVC2.Repositories
{
    public class CohortRepository
    {
        private static IConfiguration _config;

        public static void SetConfig(IConfiguration configuration)
        {
            _config = configuration;
        }

        public static SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        // static lets it be accessed anywhere and allows it to not be instatuated ie. new List <Cohort> cohort = 
        // all you have to write in your controller is CohortRepository.GetCohorts()
        public static List<Cohort> GetCohorts()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            SELECT c.Id,
                                c.Designation
                            FROM Cohort c
                        ";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Cohort> cohorts = new List<Cohort>();
                    while (reader.Read())
                    {
                        Cohort cohort = new Cohort
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Designation = reader.GetString(reader.GetOrdinal("Designation"))
                        };

                        cohorts.Add(cohort);
                    }

                    reader.Close();

                    return cohorts;
                }
            }
        }

        public static Cohort GetCohort(int id)
        {
            Cohort cohort = null;
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            SELECT c.Id,
                                c.Designation
                            FROM Cohort c WHERE c.Id = @Id
                        ";
                    cmd.Parameters.Add(new SqlParameter("@Id", id));
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        cohort = new Cohort
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Designation = reader.GetString(reader.GetOrdinal("Designation"))
                        };
                    }

                    reader.Close();
                    return cohort;
                }
            }
        }

        //post new cohort to the database
        public static Cohort CreateCohort(Cohort cohort)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Cohort (Designation)         
                                         OUTPUT INSERTED.Id                                                       
                                         VALUES (@cohortDesignation)";
                    cmd.Parameters.Add(new SqlParameter("@cohortDesignation", cohort.Designation));

                    int newId = (int)cmd.ExecuteScalar();
                    cohort.Id = newId;
                    return cohort;
                }
            }
        }

        public static bool DeleteCohort(int id)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"DELETE FROM Cohort WHERE Id = @id";
                        cmd.Parameters.Add(new SqlParameter("@id", id));

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected == 0) return false;
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        public static void UpdateCohort(Cohort cohort)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Cohort
                                            SET Name = @cohortDesignation
                                            WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@cohortDesignation", cohort.Designation));
                    cmd.Parameters.Add(new SqlParameter("@id", cohort.Id));

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
