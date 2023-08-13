using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Itmo.Dev.Asap.Core.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StudentGroups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Subjects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    MiddleName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubjectCourses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SubjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    WorkflowType = table.Column<int>(type: "integer", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectCourses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubjectCourses_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentGroupId = table.Column<Guid>(type: "uuid", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Students_StudentGroups_StudentGroupId",
                        column: x => x.StudentGroupId,
                        principalTable: "StudentGroups",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Students_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserAssociations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Discriminator = table.Column<string>(type: "text", nullable: false),
                    UniversityId = table.Column<int>(type: "integer", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAssociations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAssociations_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Assignments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SubjectCourseId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    ShortName = table.Column<string>(type: "text", nullable: false),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    MinPoints = table.Column<double>(type: "double precision", nullable: false),
                    MaxPoints = table.Column<double>(type: "double precision", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Assignments_SubjectCourses_SubjectCourseId",
                        column: x => x.SubjectCourseId,
                        principalTable: "SubjectCourses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DeadlinePenalties",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SubjectCourseId = table.Column<Guid>(type: "uuid", nullable: false),
                    SpanBeforeActivation = table.Column<long>(type: "bigint", nullable: false),
                    Discriminator = table.Column<string>(type: "text", nullable: false),
                    AbsoluteValue = table.Column<double>(type: "double precision", nullable: true),
                    Cap = table.Column<double>(type: "double precision", nullable: true),
                    Fraction = table.Column<double>(type: "double precision", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeadlinePenalties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeadlinePenalties_SubjectCourses_SubjectCourseId",
                        column: x => x.SubjectCourseId,
                        principalTable: "SubjectCourses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Mentors",
                columns: table => new
                {
                    SubjectCourseId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mentors", x => new { x.UserId, x.SubjectCourseId });
                    table.ForeignKey(
                        name: "FK_Mentors_SubjectCourses_SubjectCourseId",
                        column: x => x.SubjectCourseId,
                        principalTable: "SubjectCourses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Mentors_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SubjectCourseGroups",
                columns: table => new
                {
                    StudentGroupId = table.Column<Guid>(type: "uuid", nullable: false),
                    SubjectCourseId = table.Column<Guid>(type: "uuid", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectCourseGroups", x => new { x.SubjectCourseId, x.StudentGroupId });
                    table.ForeignKey(
                        name: "FK_SubjectCourseGroups_StudentGroups_StudentGroupId",
                        column: x => x.StudentGroupId,
                        principalTable: "StudentGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubjectCourseGroups_SubjectCourses_SubjectCourseId",
                        column: x => x.SubjectCourseId,
                        principalTable: "SubjectCourses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GroupAssignments",
                columns: table => new
                {
                    AssignmentId = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentGroupId = table.Column<Guid>(type: "uuid", nullable: false),
                    Deadline = table.Column<DateOnly>(type: "date", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupAssignments", x => new { x.StudentGroupId, x.AssignmentId });
                    table.ForeignKey(
                        name: "FK_GroupAssignments_Assignments_AssignmentId",
                        column: x => x.AssignmentId,
                        principalTable: "Assignments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GroupAssignments_StudentGroups_StudentGroupId",
                        column: x => x.StudentGroupId,
                        principalTable: "StudentGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Submissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<int>(type: "integer", nullable: false),
                    Payload = table.Column<string>(type: "text", nullable: false),
                    Rating = table.Column<double>(type: "double precision", nullable: true),
                    ExtraPoints = table.Column<double>(type: "double precision", nullable: true),
                    SubmissionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentGroupId = table.Column<Guid>(type: "uuid", nullable: false),
                    AssignmentId = table.Column<Guid>(type: "uuid", nullable: false),
                    State = table.Column<int>(type: "integer", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Submissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Submissions_GroupAssignments_StudentGroupId_AssignmentId",
                        columns: x => new { x.StudentGroupId, x.AssignmentId },
                        principalTable: "GroupAssignments",
                        principalColumns: new[] { "StudentGroupId", "AssignmentId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Submissions_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_ShortName_SubjectCourseId",
                table: "Assignments",
                columns: new[] { "ShortName", "SubjectCourseId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_SubjectCourseId",
                table: "Assignments",
                column: "SubjectCourseId");

            migrationBuilder.CreateIndex(
                name: "IX_DeadlinePenalties_SubjectCourseId",
                table: "DeadlinePenalties",
                column: "SubjectCourseId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupAssignments_AssignmentId",
                table: "GroupAssignments",
                column: "AssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Mentors_SubjectCourseId",
                table: "Mentors",
                column: "SubjectCourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_StudentGroupId",
                table: "Students",
                column: "StudentGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectCourseGroups_StudentGroupId",
                table: "SubjectCourseGroups",
                column: "StudentGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectCourses_SubjectId",
                table: "SubjectCourses",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_StudentGroupId_AssignmentId",
                table: "Submissions",
                columns: new[] { "StudentGroupId", "AssignmentId" });

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_StudentId",
                table: "Submissions",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAssociations_UniversityId",
                table: "UserAssociations",
                column: "UniversityId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserAssociations_UserId_Discriminator",
                table: "UserAssociations",
                columns: new[] { "UserId", "Discriminator" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeadlinePenalties");

            migrationBuilder.DropTable(
                name: "Mentors");

            migrationBuilder.DropTable(
                name: "SubjectCourseGroups");

            migrationBuilder.DropTable(
                name: "Submissions");

            migrationBuilder.DropTable(
                name: "UserAssociations");

            migrationBuilder.DropTable(
                name: "GroupAssignments");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Assignments");

            migrationBuilder.DropTable(
                name: "StudentGroups");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "SubjectCourses");

            migrationBuilder.DropTable(
                name: "Subjects");
        }
    }
}
