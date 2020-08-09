using Caliburn.Micro;
using MvvmDialogs;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ThirdCaliburnApp.Models;
using static ThirdCaliburnApp.Commons;

namespace ThirdCaliburnApp.ViewModels
{
    public class MainViewModel : Conductor<object>, IHaveDisplayName
    {
        #region 속성영역
        EmployeesModel employeesModel;

        //전체 Employees 리스트
        BindableCollection<EmployeesModel> employees;
        public BindableCollection<EmployeesModel> Employees 
        {
            get => employees; 
            set
            {
                employees = value;
                NotifyOfPropertyChange(() => Employees);
            }
        }

        //리스트 중 선택된 하나 Employee
        EmployeesModel selectedEmployee;
        public EmployeesModel SelectedEmployee
        {
            get => selectedEmployee;
            set
            {
                selectedEmployee = value;

                if(value != null)
                {
                    Id = value.Id;
                    EmpName = value.EmpName;
                    Salary = value.Salary;
                    DeptName = value.DeptName;
                    Destination = value.Destination;
                }

                NotifyOfPropertyChange(() => SelectedEmployee);
                NotifyOfPropertyChange(() => CanSaveEmployee);
            }
        }
        #endregion

        #region 생성자영역
        readonly IWindowManager windowManager;
        readonly IDialogService dialogService;

        int id;
        public int Id 
        { 
            get => id;
            set
            {
                id = value;
                NotifyOfPropertyChange(() => Id);
                NotifyOfPropertyChange(() => CanDeleteEmployee);
            }
        }
        string empName;
        public string EmpName 
        { 
            get => empName;
            set
            {
                empName = value;
                NotifyOfPropertyChange(() => EmpName);
                NotifyOfPropertyChange(() => CanSaveEmployee);
            } 
        }
        decimal salary;
        public decimal Salary 
        { 
            get => salary; 
            set
           {
                salary = value;
                NotifyOfPropertyChange(() => Salary);
                NotifyOfPropertyChange(() => CanSaveEmployee);
            }
        }
        string deptName;
        public string DeptName 
        { 
            get => deptName;
            set
            {
                deptName = value;
                NotifyOfPropertyChange(() => DeptName);
                NotifyOfPropertyChange(() => CanSaveEmployee);
            }
        }
        string destination;
        public string Destination 
        { 
            get => destination;
            set
            {
                destination = value;
                NotifyOfPropertyChange(() => Destination);
                NotifyOfPropertyChange(() => CanSaveEmployee);
            }
        }

        public MainViewModel(IWindowManager windowManager, IDialogService dialogService)
        {
            this.windowManager = windowManager;
            this.dialogService = dialogService;

            GetEmployees();
        }


        #endregion

        private void GetEmployees()
        {
            using (MySqlConnection conn = new MySqlConnection(Commons.CONNSTRING))
            {
                conn.Open();
                //cmd는 conn이랑 연결시켜줘야함
                MySqlCommand cmd = new MySqlCommand(EmployeesTbl.SELECT_EMPLOYEES, conn);
                //cmd.Connection = conn;
                MySqlDataReader reader = cmd.ExecuteReader();
                Employees = new BindableCollection<EmployeesModel>();
                while (reader.Read())
                {
                    var temp = new EmployeesModel
                    {
                        Id = (int)reader["id"],
                        EmpName = reader["empName"].ToString(),
                        Salary = (decimal)reader["Salary"],
                        DeptName = reader["deptName"].ToString(),
                        Destination = reader["destination"].ToString()
                    };
                    Employees.Add(temp);
                }
            }
        }
        public bool CanSaveEmployee
        {
            get
            {
                //if (string.IsNullOrEmpty(EmpName) || (Salary == 0) ||
                //    string.IsNullOrEmpty(DeptName) || string.IsNullOrEmpty(Destination)) { return false; }
                //else { return true; }

                //같은 의미
                return !(string.IsNullOrEmpty(EmpName) || (Salary == 0) ||
                   string.IsNullOrEmpty(DeptName) || string.IsNullOrEmpty(Destination));
               
            }
        }
        public void SaveEmployee()
        {
            int resultRow = 0;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(Commons.CONNSTRING))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conn;
                    if (Id == 0) //INSERT
                        cmd.CommandText = EmployeesTbl.INSERT_EMPLOYEE;
                    else
                        cmd.CommandText = EmployeesTbl.UPDATE_EMPLOYEE;

                    MySqlParameter paramEmpName = new MySqlParameter("@empName", MySqlDbType.VarChar, 45);
                    paramEmpName.Value = EmpName;
                    cmd.Parameters.Add(paramEmpName);

                    MySqlParameter paramSalary = new MySqlParameter("@Salary", MySqlDbType.Decimal);
                    paramSalary.Value = Salary;
                    cmd.Parameters.Add(paramSalary);

                    MySqlParameter paramDeptName = new MySqlParameter("@deptName", MySqlDbType.VarChar, 45);
                    paramDeptName.Value = DeptName;
                    cmd.Parameters.Add(paramDeptName);

                    MySqlParameter paramDestination = new MySqlParameter("@destination", MySqlDbType.VarChar, 45);
                    paramDestination.Value = Destination;
                    cmd.Parameters.Add(paramDestination);

                    if (Id != 0)    //INSERT문에 ID가 없어서 IF문으로 만들어주기
                    {
                        MySqlParameter paramId = new MySqlParameter("@id", MySqlDbType.Int32);
                        paramId.Value = Id;
                        cmd.Parameters.Add(paramId);
                    }

                    resultRow = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                DialogViewModel dialogViewModel = new DialogViewModel();
                dialogViewModel.DisplayName = $"{ex.Message}";
                bool? sucess = windowManager.ShowDialog(dialogViewModel);
            }
            
            if(resultRow > 0)
            {
                //MessageBox.Show("저장했습니다");
                DialogViewModel dialogViewModel = new DialogViewModel();
                dialogViewModel.DisplayName = "저장되었습니다";
                bool? sucess = windowManager.ShowDialog(dialogViewModel);

                NewEmployee();
                GetEmployees();
            }
        }
        public void NewEmployee()
        {
            Id = 0;
            EmpName = string.Empty;
            Salary = 0;
            DeptName = Destination = string.Empty;
        }

        public bool CanDeleteEmployee
        {
            get => !(Id == 0); 
        }

        public void DeleteEmployee()
        {
            int resultRow = 0;

            using (MySqlConnection conn = new MySqlConnection(Commons.CONNSTRING))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = EmployeesTbl.DELETE_EMPLOYEE;

                MySqlParameter paramId = new MySqlParameter("@id", MySqlDbType.Int32);
                paramId.Value = Id;
                cmd.Parameters.Add(paramId);

                resultRow = cmd.ExecuteNonQuery();
            }
            if(resultRow > 0)
            {
                //MessageBox.Show("삭제했습니다");
                DialogViewModel dialogViewModel = new DialogViewModel();
                dialogViewModel.DisplayName = "삭제되었습니다";
                bool? sucess = windowManager.ShowDialog(dialogViewModel);
                NewEmployee();
                GetEmployees();
            }
        }
    }
}
