﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Week09Homework
{
    public partial class FormManager : Form
    {
        public string PATH
        {
            get
            {
                var path = "c:\\class_c";
                if (false == Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                return path;
            }
        }
        public string DepartmentFullFileName
        {
            get
            {
                return Path.Combine(PATH, "department.txt");
            }
        }
        Department[] departments;
        List<Professor> professors;
        Dictionary<string, Student> students;

        List<Grade> testGrades;
        TextBox[] tbxTestScores;

        public FormManager()
        {
            InitializeComponent();

            departments = new Department[100];
            professors = new List<Professor>();
            students = new Dictionary<string, Student>();

            for (int i = 0; i < (int)YEAR.END; i++) {
                cmbYear.Items.Add(Student.YearName[(YEAR)i]);
            }

            cmbClass.Items
                .AddRange(new object[] { CLASS.A, CLASS.B, CLASS.C });

            for (int i = 0; i < (int)REG_STATUS.END; i++) {
                cmbRegStatus.Items.Add(Student.RegStatusName[(REG_STATUS)i]);
            }

            testGrades = new List<Grade>();
            tbxTestScores = new TextBox[] {
                tbxTestScore1,
                tbxTestScore2,
                tbxTestScore3,
                tbxTestScore4,
                tbxTestScore5,
                tbxTestScore6,
                tbxTestScore7,
                tbxTestScore8,
                tbxTestScore9,
            };

            //departments[0] = new Department("A001", "컴퓨터정보");

            //departments[1] = new Department("A002", "컴퓨터시스템");

            //for (int i = 0; i < departments.Length; i++) {
            //    if (departments[i] != null) {
            //        lbxDepartment.Items.Add(departments[i]);
            //    }
            //}

            if (true == File.Exists(DepartmentFullFileName))
            {
                using (var fs = new FileStream(DepartmentFullFileName, FileMode.Open))
                {
                    using (var sr = new StreamReader(fs))
                    {
                        int deptIndex = 0;
                        while (false == sr.EndOfStream)
                        {
                            var data = sr.ReadLine();
                            //A001|컴퓨터정보
                            var dept = Department.Restore(data);
                            if (dept != null && deptIndex < departments.Length)
                            {
                                departments[deptIndex++] = dept;
                                lbxDepartment.Items.Add(dept);
                            }
                        }
                    }
                }
            }

            professors.Add(new Professor("2020001", "김인하", departments[0]));

            professors.Add(new Professor("2023003", "김정석", departments[0]));

            professors.Add(new Professor("2023004", "이공전", departments[1]));

            students.Add("20240001", new Student("20240001", "김미영") {
                RegStatus = REG_STATUS.ENROLLED,
                Year = YEAR.ONE,
                // LINQ = array, collection, query
                Dept = departments.FirstOrDefault(m=> m.Code == "A001"),
                AdvisorNumber = "2020001",
                Class = CLASS.B,
                Address = "인천 남구 용현동 100",
                Contact = "032-870-0000"
            });

            students["20240001"].SetBirthInfo(2004, 01, 01);

            foreach (var student in students) {
                if (student.Value != null) {
                    lbxDictionary.Items.Add(student.Value);
                }
            }
        }

        private void btnRegisterDepartment_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbxDepartmentCode.Text)) {
                MessageBox.Show("학과코드 입력");
                tbxDepartmentCode.Focus();
                return;
            }

            if (string.IsNullOrEmpty(tbxDepartmentName.Text)) {
                MessageBox.Show("학과이름 입력");
                tbxDepartmentName.Focus();
                return;
            }

            int index = -1;
            for (int i = 0; i < departments.Length; i++) {
                if (departments[i] == null) {
                    if (index < 0) {
                        index = i;
                    }
                    //break;
                } else {
                    if (departments[i].GetCode() == tbxDepartmentCode.Text) {
                        MessageBox.Show("중복 학과코드");
                        tbxDepartmentCode.Focus();
                        return;
                    }
                }
            }

            if (index < 0) {
                MessageBox.Show("신규 학과를 개설할 수 없습니다.");
                return;
            }

            Department dept = new Department(tbxDepartmentCode.Text, tbxDepartmentName.Text);

            departments[index] = dept;

            lbxDepartment.Items.Add(dept);

            // file - append
            using (var fs = new FileStream(DepartmentFullFileName, FileMode.Append)) // open
            {
                using (var sw = new StreamWriter(fs))
                {
                    // append
                    sw.WriteLine(dept.Record);
                }
            } // close
        }

        private void btnRemoveDepartment_Click(object sender, EventArgs e)
        {
            if (lbxDepartment.SelectedIndex < 0) {
                MessageBox.Show("삭제할 학과를 선택");
                return;
            }

            if (lbxDepartment.SelectedItem is Department) {
                var target = (Department)lbxDepartment.SelectedItem;

                for (int i = 0; i < departments.Length; i++) {
                    if (departments[i] != null && departments[i] == target) {
                        departments[i] = null;
                        break;
                    }
                }

                lbxDepartment.Items.RemoveAt(lbxDepartment.SelectedIndex);
                lbxDepartment.SelectedIndex = -1;

                // file - write
                try
                {
                    using (var fs = new FileStream(DepartmentFullFileName, FileMode.Create)) // open
                    {
                        using (var sw = new StreamWriter(fs))
                        {
                            // write
                            foreach (var dept in departments)
                            {
                                sw.WriteLine(dept.Record);
                            }
                        }
                    } // close
                }
                catch (NullReferenceExcetion ex)
                {
                    Console.WriteLine("빈 배열" + ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                finally
                {
                    Console.WriteLine("파일 삭제 처리 끝");
                }
            }
        }

        private void tabMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabMain.SelectedIndex) {
                case 0:
                    break;
                case 1:
                    cmbProfessorDepartment.Items.Clear();
                    foreach (var department in departments) {
                        if (department != null) {
                            cmbProfessorDepartment.Items.Add(department);
                        }
                    }

                    cmbProfessorDepartment.SelectedIndex = -1;
                    lbxProfessor.Items.Clear();
                    break;
                case 2:
                    cmbDepartment.Items.Clear();
                    foreach (var department in departments) {
                        if (department != null) {
                            cmbDepartment.Items.Add(department);
                        }
                    }

                    ClearStudentInfo();
                    break;
                case 3:
                    ClearTestScoreInfo();
                    break;
                default:
                    break;
            }
        }

        private void ClearTestScoreInfo()
        {
            lblTestName.Text = string.Empty;

            foreach (var textbox in tbxTestScores) {
                textbox.Text = string.Empty;
            }
        }

        private void cmbProfessorDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbProfessorDepartment.SelectedIndex < 0) {
                return;
            }

            lbxProfessor.Items.Clear();

            var department = cmbProfessorDepartment.SelectedItem as Department;

            if (department != null) {
                foreach (var professor in professors) {
                    if (professor != null && professor.Dept.Code == department.Code) {
                        lbxProfessor.Items.Add(professor);
                    }
                }
            }
        }

        private void btnRegisterProfessor_Click(object sender, EventArgs e)
        {
            if (cmbProfessorDepartment.SelectedIndex < 0) {
                MessageBox.Show("학과를 선택");
                cmbProfessorDepartment.Focus();
                return;
            }

            if (false == cmbProfessorDepartment.SelectedItem is Department dept) {
                MessageBox.Show("학과정보에 이상 발생");
                cmbProfessorDepartment.Focus();
                return;
            }

            if (string.IsNullOrEmpty(tbxProfessorNumber.Text)) {
                MessageBox.Show("교수번호 입력");
                tbxProfessorNumber.Focus();
                return;
            }

            if (string.IsNullOrEmpty(tbxProfessorName.Text)) {
                MessageBox.Show("교수이름 입력");
                tbxProfessorName.Focus();
                return;
            }

            for (int i = 0; i < professors.Count; i++) {
                if (professors[i].Number == tbxProfessorNumber.Text) {
                    MessageBox.Show("중복 교수코드");
                    tbxProfessorNumber.Focus();
                    return;
                }
            }

            Professor professor = new Professor(tbxProfessorNumber.Text, tbxProfessorName.Text, dept);

            professors.Add(professor);
            lbxProfessor.Items.Add(professor);
        }

        private void btnRemoveProfessor_Click(object sender, EventArgs e)
        {
            if (lbxProfessor.SelectedIndex < 0) {
                MessageBox.Show("삭제할 교수를 선택");
                return;
            }

            if (lbxProfessor.SelectedItem is Professor) {
                var target = (Professor)lbxProfessor.SelectedItem;

                for (int i = 0; i < professors.Count; i++) {
                    if (professors[i] != null && professors[i] == target) {
                        professors.RemoveAt(i);
                        break;
                    }
                }

                lbxProfessor.Items.Remove(target);
                lbxProfessor.SelectedIndex = -1;
            }
        }

        private void cmbDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbAdvisor.Items.Clear();

            if (cmbDepartment.SelectedIndex < 0) {
                return;
            }

            if (false == cmbDepartment.SelectedItem is Department dept) {
                return;
            }

            foreach (var professor in professors) {
                if (professor != null && professor.Dept.Code == dept.Code) {
                    cmbAdvisor.Items.Add(professor);
                }
            }

            cmbAdvisor.SelectedIndex = -1;
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            ClearStudentInfo();
            lbxDictionary.SelectedIndex = -1;
        }

        private void ClearStudentInfo()
        {
            tbxNumber.Text = "20";
            tbxName.Text = string.Empty;
            tbxBirthYear.Text = "20";
            tbxBirthMonth.Text = "";
            tbxBirthDay.Text = "";
            cmbDepartment.SelectedIndex = -1;
            cmbAdvisor.SelectedIndex = -1;
            cmbYear.SelectedIndex = -1;
            cmbClass.SelectedIndex = -1;
            cmbRegStatus.SelectedIndex = -1;
            tbxAddress.Text = string.Empty;
            tbxContact.Text = string.Empty;

            tbxNumber.ReadOnly = false;
            selectedStudent = null;
            btnRegister.Text = "등록";
        }

        Student selectedStudent = null;
        private void btnRegister_Click(object sender, EventArgs e)
        {
            if (selectedStudent == null) {
                RegisterStudent();
            } else {
                UpdateStudent();          //call
            }
        }

        private void RegisterStudent()
        {
            var number = tbxNumber.Text.Trim();

            if (string.IsNullOrEmpty(number) || number.Length != 8) {
                tbxNumber.Focus();
                return;
            }

            if (string.IsNullOrEmpty(tbxName.Text) || tbxName.Text.Trim().Length < 2) {
                tbxName.Focus();
                return;
            }

            //for, 성능 떨어짐
            for (int i = 0; i < students.Count; i++) {
                var pair = students.ElementAt(i);
                if (pair.Key == number) {
                    tbxNumber.Focus();
                    return;
                }
            }

            //foreach
            foreach (var pair in students) {
                if (pair.Key == number) {
                    tbxNumber.Focus();
                    return;
                }
            }

            //실제 많이 사용하는 방법1
            if (true == students.ContainsKey(number)) {
                tbxNumber.Focus();
                return;
            }

            Student student = new Student(number, tbxName.Text.Trim());

            int birthYear, birthMonth;// birthDay;
            if (true == int.TryParse(tbxBirthYear.Text, out birthYear)) {
                if (birthYear < 1900 || 9000 < birthYear) {
                    tbxBirthYear.Focus();
                    return;
                }
            } else {
                tbxBirthYear.Focus();
                return;
            }

            if (true == int.TryParse(tbxBirthMonth.Text, out birthMonth)) {
                if (birthMonth < 1 || 12 < birthMonth) {
                    tbxBirthMonth.Focus();
                    return;
                }
            } else {
                tbxBirthMonth.Focus();
                return;
            }

            if (true == int.TryParse(tbxBirthDay.Text, out int birthDay)) {
                //2월, 달 처리등은 추후 해볼것
                if (birthDay < 0 || 31 < birthDay) {
                    tbxBirthDay.Focus();
                    return;
                }
            } else {
                tbxBirthDay.Focus();
                return;
            }

            student.SetBirthInfo(birthYear, birthMonth, birthDay);

            if (cmbDepartment.SelectedIndex < 0) {
                student.Dept = null;
            } else {
                student.Dept = (cmbDepartment.SelectedItem as Department);
            }

            if (cmbAdvisor.SelectedIndex < 0) {
                student.AdvisorNumber = null;
            } else {
                student.AdvisorNumber = (cmbAdvisor.SelectedItem as Professor).Number;
            }

            if (cmbYear.SelectedIndex < (int)YEAR.END) {
                student.Year = (YEAR)cmbYear.SelectedIndex;
            }

            if (cmbClass.SelectedIndex < 0) {
                cmbClass.Focus();
                return;
            }
            student.Class = (CLASS)cmbClass.SelectedIndex;

            if (cmbRegStatus.SelectedIndex < 0) {
                cmbRegStatus.Focus();
                return;
            }
            student.RegStatus = (REG_STATUS)cmbRegStatus.SelectedIndex;

            student.Address = tbxAddress.Text.Trim();
            student.Contact = tbxContact.Text.Trim();

            students[number] = student;
            lbxDictionary.Items.Add(student);
        }

        private void UpdateStudent()
        {
            if (string.IsNullOrEmpty(tbxName.Text) || tbxName.Text.Trim().Length < 2) {
                tbxName.Focus();
                return;
            }

            if (true == int.TryParse(tbxBirthYear.Text, out int birthYear)) {
                if (birthYear < 1900 || 9000 < birthYear) {
                    tbxBirthYear.Focus();
                    return;
                }
            } else {
                tbxBirthYear.Focus();
                return;
            }


            if (true == int.TryParse(tbxBirthMonth.Text, out int birthMonth)) {
                if (birthMonth < 1 || 12 < birthMonth) {
                    tbxBirthMonth.Focus();
                    return;
                }
            } else {
                tbxBirthMonth.Focus();
                return;
            }

            if (true == int.TryParse(tbxBirthDay.Text, out int birthDay)) {
                //2월, 달 처리등은 추후 해볼것
                if (birthDay < 0 || 31 < birthDay) {
                    tbxBirthDay.Focus();
                    return;
                }
            } else {
                tbxBirthDay.Focus();
                return;
            }

            if (cmbDepartment.SelectedIndex < 0) {
                //cmbDepartment.Focus();
                return;
            }

            if (cmbYear.SelectedIndex < 0) {
                cmbYear.Focus();
                return;
            }

            if (false == int.TryParse(cmbYear.SelectedItem.ToString(), out int year)) {
                cmbYear.Focus();
                return;
            }

            if (cmbClass.SelectedIndex < 0) {
                cmbClass.Focus();
                return;
            }

            if (cmbRegStatus.SelectedIndex < 0) {
                cmbRegStatus.Focus();
                return;
            }

            selectedStudent.SetBirthInfo(birthYear, birthMonth, birthDay);

            if (cmbDepartment.SelectedIndex < 0) {
                selectedStudent.Dept = null;
            } else {
                selectedStudent.Dept = (cmbDepartment.SelectedItem as Department);
            }

            if (cmbAdvisor.SelectedIndex < 0) {
                selectedStudent.AdvisorNumber = null;
            } else {
                selectedStudent.AdvisorNumber = (cmbAdvisor.SelectedItem as Professor).Number;
            }

            selectedStudent.Year = (YEAR)year;
            selectedStudent.Class = (CLASS)cmbClass.SelectedIndex;
            selectedStudent.RegStatus = (REG_STATUS)cmbRegStatus.SelectedIndex;
            selectedStudent.Address = tbxAddress.Text.Trim();
            selectedStudent.Contact = tbxContact.Text.Trim();

            MessageBox.Show("수정완료");
            lbxDictionary.SelectedIndex = -1;
            ClearStudentInfo();
        }

        private void lbxDictionary_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbxDictionary.SelectedIndex < 0) {
                return;
            }

            var student = (lbxDictionary.SelectedItem as Student);

            ClearStudentInfo();

            if (student != null) {
                DisplaySelectedStudent(student);
            }
        }

        private void DisplaySelectedStudent(Student student)
        {
            selectedStudent = student;
            tbxNumber.ReadOnly = true;

            tbxNumber.Text = student.Number;
            tbxName.Text = student.Name;
            tbxBirthYear.Text = student.BirthInfo.Year.ToString();
            tbxBirthMonth.Text = student.BirthInfo.Month.ToString();
            tbxBirthDay.Text = student.BirthInfo.Day.ToString();

            for (int i = 0; i < cmbDepartment.Items.Count; i++) {
                if ((cmbDepartment.Items[i] as Department).Code == student.Dept.Code) {
                    cmbDepartment.SelectedIndex = i;
                    break;
                }
            }

            for (int i = 0; i < cmbAdvisor.Items.Count; i++) {
                if ((cmbAdvisor.Items[i] as Professor).Number == student.AdvisorNumber) {
                    cmbAdvisor.SelectedIndex = i;
                    break;
                }
            }

            for (int i = 0; i < cmbYear.Items.Count; i++) {
                if (cmbYear.Items[i].ToString() == student.Year.ToString()) {
                    cmbYear.SelectedIndex = i;
                    break;
                }
            }

            for (int i = 0; i < cmbClass.Items.Count; i++) {
                if (i == (int)student.Class) {
                    cmbClass.SelectedIndex = i;
                    break;
                }
            }

            for (int i = 0; i < cmbRegStatus.Items.Count; i++) {
                if (i == (int)student.RegStatus) {
                    cmbRegStatus.SelectedIndex = i;
                    break;
                }
            }

            tbxAddress.Text = selectedStudent.Address;
            tbxContact.Text = selectedStudent.Contact;

            btnRegister.Text = "수정";
        }

        private void btnTestSearchStudent_Click(object sender, EventArgs e)
        {
            ClearTestScoreInfo();

            var number = tbxTestNumber.Text.Trim();
            if (string.IsNullOrEmpty(number) || number.Length != 8) {
                tbxNumber.Focus();
                return;
            }

            selectedStudent = SearchStudentByNumber(tbxTestNumber.Text);

            if (selectedStudent == null) {
                MessageBox.Show($"{number}학번의 학생 정보가 없음");
                tbxTestNumber.Focus();
                return;
            }

            lblTestName.Text = selectedStudent.Name;

            Grade grade = SearchGradeByNumber(selectedStudent.Number);

            if (grade != null) {
                for (int i = 0; i < grade.Count() && i < tbxTestScores.Length; i++) {
                    tbxTestScores[i].Text = grade.Get(i).ToString("0.0");
                }
            }
        }

        Grade SearchGradeByNumber(string number)
        {
            foreach (Grade grade in testGrades) {
                if (grade.StudentNumber == number) {
                    return grade;
                }
            }

            return null;
        }

        private Student SearchStudentByNumber(string number)
        {
            if (students.ContainsKey(number)) {
                return students[number];
            } else {
                return null;
            }
        }

        private void btnTestRegScore_Click(object sender, EventArgs e)
        {
            //추가코드
            lblTestTotalCount.Text = "";
            lblTestAverage.Text = "";

            if (selectedStudent == null) {
                tbxTestNumber.Focus();
                return;
            }

            for (int i = 1; i < tbxTestScores.Length; i++) {
                if (true == string.IsNullOrEmpty(tbxTestScores[i - 1].Text) && false == string.IsNullOrEmpty(tbxTestScores[i].Text)) {
                    tbxTestScores[i - 1].Focus();
                    return;
                }
            }

            var grade = SearchGradeByNumber(selectedStudent.Number);

            if (grade == null) {
                grade = new Grade(selectedStudent.Number);
            }

            grade.Clear();

            double temp;
            for (int i = 0; i < tbxTestScores.Length; i++) {
                if (string.IsNullOrEmpty(tbxTestScores[i].Text)) {
                    break;
                }

                if (false == double.TryParse(
                    tbxTestScores[i].Text, out temp)) {
                    tbxTestScores[i].Focus();
                    return;
                }
                grade.Add(temp);
            }

            testGrades.Add(grade);

            lblTestTotalCount.Text = grade.Count().ToString();

            double average = grade.Average();
            lblTestAverage.Text = average.ToString("F1");
        }
    }
}
