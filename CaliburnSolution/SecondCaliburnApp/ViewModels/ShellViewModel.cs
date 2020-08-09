using Caliburn.Micro;
using MySql.Data.MySqlClient;
using SecondCaliburnApp.Helpers;
using SecondCaliburnApp.Models;
using System;
using System.Linq;
using System.Windows.Documents.DocumentStructures;

namespace SecondCaliburnApp.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHaveDisplayName
    {
        public override string DisplayName { get; set; }
        string firstName;
        public string FirstName
        {
            get => firstName;
            set
            {
                firstName = value;
                NotifyOfPropertyChange(() => FirstName);
                NotifyOfPropertyChange(() => FullName);
                NotifyOfPropertyChange(() => CanClearName);
            }
        }
        string lastName;
        public string LastName
        {
            get => lastName;
            set
            {
                lastName = value;
                NotifyOfPropertyChange(() => LastName);
                NotifyOfPropertyChange(() => FullName);
                NotifyOfPropertyChange(() => CanClearName);

            }
        }

        public string FullName
        {
            get => $"{FirstName} {LastName}";       //string.Format("{0} {1}", FirstName, LastName);
        }

        public ShellViewModel()
        {
            DisplayName = "Second Caliburn App";
            FirstName = "HaYoung";

            People = new BindableCollection<PersonModel>();
            InitCombobox();
            //People.Add(new PersonModel { LastName = "Garam", FirstName = "Kim" });
            //People.Add(new PersonModel { LastName = "Yeji", FirstName = "Yeo" });
            //People.Add(new PersonModel { LastName = "Changseob", FirstName = "Lim" });
        }

        private void InitCombobox()
        {
            People.Add(new PersonModel { LastName= "선택", FirstName = "" });
            using (MySqlConnection conn = new MySqlConnection(Commons.STRCONNSTRING))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(Commons.SELECTPEOPLEQUERY, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while(reader.Read())
                {
                    var temp = new PersonModel
                    {
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString()
                    };
                    People.Add(temp);
                }
            }
            SelectedPerson = People.Where(v => v.LastName.Contains("선택")).First();
        }


        //콤보박스 사람 리스트
        public BindableCollection<PersonModel> People { get; set; }

        PersonModel selectedPerson;
        
        public PersonModel SelectedPerson
        {
            get => selectedPerson;
            set
            {
                selectedPerson = value;
                //뷰모델 안의 라스트네임
                this.LastName = selectedPerson.LastName;
                this.FirstName = selectedPerson.FirstName;

                NotifyOfPropertyChange(() => SelectedPerson);   //값이 변했다고 알려주는것 필수!
                NotifyOfPropertyChange(() => CanClearName);
            }
        }

        public void ClearName()
        {
            this.FirstName = this.LastName = string.Empty;
        }

        public bool CanClearName
        {
            //if (string.IsNullOrEmpty(FirstName) && string.IsNullOrEmpty(LastName))
            //    return false;
            //else
            //    return true;

            //같은 의미
            get => !(string.IsNullOrEmpty(FirstName) && string.IsNullOrEmpty(LastName));
        }

        // Usercontrol FirstChildView Load
        public void LoadPageOne()
        {
            ActivateItem(new FirstChildViewModel());
            
        }

        // UeserControl SecondChildView Load
        public void LoadPageTwo()
        {
            ActivateItem(new SecondChildViewModel());
        }
    }
}
