using Dapper;
using JobManagementApi.Models.DTOS;
using Npgsql;
using System.Text;

namespace JobManagementApi.Repos
{
    public class JobRepo : IJobRepo
    {
        public async Task AddCV(int userId, CVTableModel cv, NpgsqlConnection connection, NpgsqlTransaction transaction)
        {
            var query = @"INSERT INTO cvs (user_id, education , start_date, end_date, degree, profession)
                                  values  (@userId, @Education, @StartDate, @EndDate, @Degree, @Profession)";
            await connection.ExecuteAsync(query,
                new 
                {
                    userId = userId,
                    Education = cv.Education,
                    StartDate = cv.StartDate,
                    EndDate = cv.EndDate,
                    Degree = cv.Degree,
                    Profession = cv.Profession
                },
                transaction);
        }

        public async Task AddSkills(int userId, SkillsCVTableModel skills, NpgsqlConnection connection, NpgsqlTransaction transaction)
        {
            foreach (var skill in skills.Skills)
            {
                var query = @"INSERT INTO skills_cv (user_id, skill, experience)
                                          values (@userId, @Skill, @Experience)";
                await connection.ExecuteAsync(query, new
                {
                    userId = userId,
                    Skill = skill.SkillId,
                    Experience = skill.Experience
                },
                transaction);
            }
            
            
        }

        public async Task<bool> CVExists(int userId, NpgsqlConnection connection, NpgsqlTransaction transaction)
        {
            var query = "SELECT COUNT(*) FROM cvs WHERE user_id = @userId";
            int numEmails = await connection.QuerySingleOrDefaultAsync<int>(query, new { userId }, transaction);
            return numEmails > 0;
        }

        public async Task DeleteCV(int userId, NpgsqlConnection connection, NpgsqlTransaction transaction)
        {
            var cvQuery = "DELETE FROM cvs WHERE user_id = @userId";
            var skillsQuery = "DELETE FROM skills_cv WHERE user_id = @userId";
            await connection.ExecuteAsync(cvQuery, new { userId  = userId }, transaction);
            await connection.ExecuteAsync(skillsQuery, new { userId = userId }, transaction);
        }

        public async Task DeleteSkill(int userId, SkillIds skills, NpgsqlConnection connection, NpgsqlTransaction transaction)
        {
            foreach(var skill in skills.Skills)
            {
                var cvQuery = "DELETE FROM skills_cv WHERE user_id = @userId and skill = @SkillId";
                await connection.ExecuteAsync(cvQuery, new {userId, SkillId = skill}, transaction);
            }
            
        }

        public async Task EditCV(int userId, CV cv, NpgsqlConnection connection, NpgsqlTransaction transaction)
        {
            var cvsQuery = @"UPDATE cvs SET 
                            education = @Education, start_date = @StartDate,
                            end_date = @EndDate, degree = @Degree, profession = @Profession
                            WHERE user_id = @userId";
            await connection.ExecuteAsync(cvsQuery, new
            {
                Education = cv.Education,
                StartDate = cv.StartDate,
                EndDate = cv.EndDate,
                Degree = cv.Degree,
                Profession = cv.Profession,
                userId = userId
            }, transaction);


            foreach (var skill in cv.Skills)
            {
                var skillsQuery = @"UPDATE skills_cv SET 
                                experience = @Experience
                                WHERE user_id = @userId AND skill = @Skill";
                await connection.ExecuteAsync(skillsQuery, new
                {
                    Experience = skill.Experience,
                    userId = userId,
                    Skill = skill.SkillId
                }, transaction);
            }
        }

        public async Task<MultipleChoiseLists> GetMultipleChoiseLists(NpgsqlConnection connection)
        {
            MultipleChoiseLists res = new MultipleChoiseLists();
            res.Skills = (await connection.QueryAsync<Skill>(
                            @"SELECT skill_id AS SkillId, skill_name AS SkillName FROM skills")).ToList();
            res.Degrees = (await connection.QueryAsync<Degree>(
                            @"SELECT degree_id AS DegreeId, degree_name AS DegreeName FROM degrees")).ToList();
            res.Education = (await connection.QueryAsync<Education>(
                            @"SELECT education_id AS EducationId, education_name AS EducationName FROM educations")).ToList();
            res.Professions = (await connection.QueryAsync<Profession>(
                            @"SELECT profession_id AS ProfessionId, profession_name AS ProfessionName FROM professions")).ToList();
            return res;
        }

    }
}
