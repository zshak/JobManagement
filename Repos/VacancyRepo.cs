using Dapper;
using JobManagementApi.Models.DTOS;
using Npgsql;

namespace JobManagementApi.Repos
{
    public class VacancyRepo : IVacancyRepo
    {
        public async Task<int> AddVacancy(int managerId, JobsVacancyModel jobsVacancy, NpgsqlConnection connection, NpgsqlTransaction transaction)
        {
            const string query = @"INSERT INTO jobs (manager_id, job_title, expiration_date)
                                    VALUES (@ManagerId, @JobTitle, @ExpirationDate)
                                    RETURNING job_id";
            var job_id = await connection.QuerySingleOrDefaultAsync<int>(query, new
            {
                ManagerId = managerId,
                JobTitle = jobsVacancy.JobTitle,
                ExpirationDate = jobsVacancy.ExpirationDate
            }, transaction);
            return job_id;
        }

        public async Task AddVacancySkills(int jobId, JobsReqSkillsModel jobsSkills, NpgsqlConnection connection, NpgsqlTransaction transaction)
        {
            foreach (var skill in jobsSkills.Skills)
            {
                var query = @"INSERT INTO job_requiered_skills (job_id, skill, weight, experience)
                                                        VALUES (@JobId, @Skill, @Weight, @Experience)";
                await connection.ExecuteAsync(query, new
                {
                    JobId = jobId,
                    Skill = skill.SkillId,
                    Weight = skill.Weight,
                    Experience = skill.Experience
                }, transaction);
            }
        }

        public async Task ApplyUserForVacancy(int userId, int jobId, NpgsqlConnection connection, NpgsqlTransaction transaction)
        {
            const string query = @"INSERT INTO applications (job_id, user_id)
                                            VALUES (@jobId, @userId)";
            await connection.ExecuteAsync(query, new { jobId, userId }, transaction);
        }

        public async Task<List<int>> GetUsersAppliedForJob(int jobId, NpgsqlConnection connection, NpgsqlTransaction transaction)
        {
            const string query = @"SELECT user_id FROM applications WHERE job_id = @jobId";
            return (await connection.QueryAsync<int>(query, new { jobId }, transaction)).ToList();
        }

        public async Task<List<VacancySkillModel>> GetJobRequiredSkills(int jobId, NpgsqlConnection connection, NpgsqlTransaction transaction)
        {
            const string query = @"SELECT skill AS SkillId, weight AS Weight, experience AS Experience
                                    FROM job_requiered_skills WHERE job_id = @jobId";
            return (await connection.QueryAsync<VacancySkillModel>(query, new { jobId }, transaction)).ToList();
        }

        public async Task<List<SkillTableModel>> GetUserSkills(int userId, NpgsqlConnection connection, NpgsqlTransaction transaction)
        {

            const string query = @"SELECT skill as SkillId, experience as Experience
                                    FROM skills_cv where user_id = @userId";
            return (await connection.QueryAsync<SkillTableModel>(query, new { userId }, transaction)).ToList();
        }

        public async Task<List<JobsTableModel>> GetEmployerVacancies(int userId, NpgsqlConnection connection, NpgsqlTransaction transaction)
        {
            const string query = @"SELECT job_id AS JobId, manager_id AS ManagerId, job_title AS JobTitle, expiration_date AS ExpirationDate
                                    FROM jobs where manager_id = @userId";
            return (await connection.QueryAsync<JobsTableModel>(query, new { userId }, transaction)).ToList();
        }

        public async Task<int> UpdateVacancy(int jobId, int userId, JobsVacancyModel vacancy, NpgsqlConnection connection,
            NpgsqlTransaction transaction)
        {
            const string query = @"UPDATE jobs SET 
                            job_title = @JobTitle, expiration_date = @ExpirationDate
                            WHERE job_id = @JobId AND manager_id = @UserId";
            int numRowsAffected = await connection.ExecuteAsync(query, new
            {
                JobTitle = vacancy.JobTitle,
                ExpirationDate = vacancy.ExpirationDate,
                JobId = jobId,
                UserId = userId,
            }, transaction);
            return numRowsAffected;
        }

        public async Task<int> UpdateVacancyReqSkills(int jobId, VacancySkillModel skill, NpgsqlConnection connection, NpgsqlTransaction transaction)
        {
            const string query = @"UPDATE job_requiered_skills SET 
                                weight = @Weight, experience = @Experience
                                WHERE job_id = @JobId and skill = @Skill";
            int numRowsAffected = await connection.ExecuteAsync(query, new
            {
                Skill = skill.SkillId,
                Weight = skill.Weight,
                Experience = skill.Experience,
                JobId = jobId
            }, transaction);
            return numRowsAffected;
        }

        public async Task<int> DeleteVacancySkill(int jobId, int skillId, NpgsqlConnection connection, NpgsqlTransaction transaction)
        {
            const string query = @"DELETE FROM job_requiered_skills 
                                    WHERE job_id = @JobId AND skill = @Skill";
            int numRowsAffected = await connection.ExecuteAsync(query ,new
            {
                Skill = skillId,
                JobId = jobId
            }, transaction);
            return numRowsAffected;
        }

        public async Task<int> GetUserByJobId(int jobId, int userId, NpgsqlConnection connection)
        {
            const string query = @"SELECT user_id FROM applications WHERE user_id = @userId AND job_id = @jobId";
            var user = await connection.QuerySingleOrDefaultAsync<int>(query, new { userId, jobId });
            if (user == 0) return -1;
            return user;
        }
    }
}
