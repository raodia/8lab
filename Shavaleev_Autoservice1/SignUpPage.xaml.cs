using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Shavaleev_Autoservice1
{
    /// <summary>
    /// Логика взаимодействия для Page1.xaml
    /// </summary>
    public partial class SignUpPage : Page
    {
        private ClientService _currentClientService = new ClientService();

        private Service _currentService = new Service();
        public SignUpPage(Service SelectedService)
        {
            InitializeComponent();
            this._currentService = SelectedService;
            DataContext = _currentService;

            var _currentClient = ShaveleevAutoserviceEntities.GetContext().Client.ToList();

            ComboClient.ItemsSource = _currentClient;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (ComboClient.SelectedItem == null)
                errors.AppendLine("Укажите ФИО клиента");
            if (StartDate.Text == "")
                errors.AppendLine("Укажите дату услуги");
            if (TBStart.Text == "" || TBEnd.Text=="")
                errors.AppendLine("Укажите время начала услуги");
            string[] parts = TBStart.Text.Split(':');
            if (Convert.ToInt32(parts[0]) > 23 || Convert.ToInt32(parts[0]) < 0 || Convert.ToInt32(parts[1]) > 59 || Convert.ToInt32(parts[1]) < 0)
                errors.AppendLine("Введено некорректное время");
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            _currentClientService.ClientID = ComboClient.SelectedIndex + 1;
            _currentClientService.ServiceID = _currentService.id;
            _currentClientService.StartTime = Convert.ToDateTime(StartDate.Text + " " + TBStart.Text);

            if (_currentClientService.ID == 0)
                ShaveleevAutoserviceEntities.GetContext().ClientService.Add(_currentClientService);

            try
            {
                ShaveleevAutoserviceEntities.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена");
                Manager.MainFrame.GoBack();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void TBStart_TextChanged(object sender, TextChangedEventArgs e)
        {
            string s = TBStart.Text;
            if (s.Length < 3 || !s.Contains(':') || s[s.Length-1]==':')
                TBEnd.Text = "";
            else
            {
                string[] start = s.Split(':');
                int startHour = Convert.ToInt32(start[0].ToString()) * 60;
                int startMin = Convert.ToInt32(start[1].ToString());

                int sum = startHour + startMin + _currentService.Duration;

                int EndHour = sum / 60;
                int EndMin = sum % 60;
                s = EndHour.ToString() + ":" + EndMin.ToString();
                TBEnd.Text = s;
            }
        }
    }
}

