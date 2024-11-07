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
    /// Логика взаимодействия для AddEditPage.xaml
    /// </summary>
    public partial class AddEditPage : Page
    {
        private Service _currentService = new Service();


        public AddEditPage(Service SelectedService)
        {
            InitializeComponent();

            if (SelectedService != null)
                _currentService = SelectedService;

            DataContext = _currentService;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(_currentService.Title))
                errors.AppendLine("Укажите название услуги");

            if (_currentService.Cost == 0)
                errors.AppendLine("Укажите стоимость услуги");

            if (_currentService.DiscountInt < 0 && _currentService.DiscountInt > 100)
                errors.AppendLine("Укажите скидку");


            if (_currentService.Duration == 0 || string.IsNullOrWhiteSpace(_currentService.Duration.ToString())) 
                errors.AppendLine("Укажите длительность услуги");

            if (_currentService.Duration > 240)
            {
                errors.AppendLine("Длительность не может быть больше 240 минут");
            }

            if (string.IsNullOrWhiteSpace(_currentService.DiscountInt.ToString()))
                _currentService.DiscountInt = 0;

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            var allServices = ShaveleevAutoserviceEntities.GetContext().Service.ToList();
            allServices = allServices.Where(p => p.Title == _currentService.Title).ToList();

            if (allServices.Count == 0)
            {
                if (_currentService.id == 0)
                    ShaveleevAutoserviceEntities.GetContext().Service.Add(_currentService);
            try
            {
                ShaveleevAutoserviceEntities.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена");
                Manager.MainFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            }

        else {
                MessageBox.Show("Такая услуга уже существует");
        }
        
        }


    }
}
