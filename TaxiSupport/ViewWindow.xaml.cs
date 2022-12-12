using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Word = Microsoft.Office.Interop.Word;

namespace TaxiSupport
{
    /// <summary>
    /// Логика взаимодействия для ViewWindow.xaml
    /// </summary>
    public partial class ViewWindow : Window
    {
        User user;
        IEnumerable<Appeal> AppealList;
        /// <summary>
        /// Загрузка логотипа и вывод данных на форму
        /// </summary>
        /// <param name="user"> Активный пользователь</param>
        public ViewWindow(User user)
        {
            InitializeComponent();
            Logo_auth.Source = new BitmapImage(new Uri(System.IO.Path.GetFullPath("../../Resource/Oe0jPt9jLVU.jpg")));
            filterComboBox.ItemsSource = DataBase.GetContext().Importance.ToList();
            this.user = user;
            DataContext = DataBase.GetContext().Appeal.Where(p=> p.status == 1).ToList();
            sortComboBox.SelectedIndex = 1;
            UpdateData();
        }
        /// <summary>
        /// Метод, для отрисовки данных. id == 1 это для вывода только открытых обращений.
        /// </summary>
        public void UpdateData()
        {
            DataBase.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
            AppealList = DataBase.GetContext().Appeal.Where(p => p.status == 1).ToList();
            if (!String.IsNullOrWhiteSpace(SearchBox.Text))
            {
                AppealList = AppealList.Where(p => p.description.Contains(SearchBox.Text)).ToList();
            }
            if (filterComboBox.SelectedItem != null)
            {
                AppealList = AppealList.Where(p => p.Importance1 == (filterComboBox.SelectedItem as Importance)).ToList();
            }
            switch (sortComboBox.SelectedIndex)
            {
                case 0:
                    AppealList = AppealList.OrderBy(p => p.importance).ToList();
                    break;
                case 1:
                    AppealList = AppealList.OrderByDescending(p => p.importance).ToList();
                    break;
                case 2:
                    AppealList = AppealList.OrderBy(p => p.date).ToList();
                    break;
                case 3:
                    AppealList = AppealList.OrderByDescending(p => p.date).ToList();
                    break;
            }
            listBox.ItemsSource = AppealList;
        }
        /// <summary>
        /// Создаем окно просмотра, передавая выбранное обращение и активного пользователя
        /// </summary>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AppealViewWindow appealViewWindow = new AppealViewWindow(listBox.SelectedItem as Appeal, user);
            this.Hide();
            appealViewWindow.Owner = this;
            appealViewWindow.Show();
        }
        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewButton.IsEnabled = true;
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateData();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            this.Owner.Show();
        }
        DateTime dateTime = new DateTime(2022, 6, 18, 8, 0, 0);
        private void ReportButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Word document(*.docx) | *.docx";
            object oMissing = System.Reflection.Missing.Value;
            //Документ
            Word.Application word_app = new Word.Application();
            word_app.Visible = true;
            Word.Document doc = word_app.Documents.Add(ref oMissing, ref oMissing, ref oMissing, ref oMissing);
            Word.Range wordrange = doc.Range(ref oMissing, ref oMissing);
            doc.PageSetup.LeftMargin = word_app.CentimetersToPoints(1);
            doc.PageSetup.RightMargin = word_app.CentimetersToPoints(1);
            doc.PageSetup.BottomMargin = word_app.CentimetersToPoints(2);
            doc.PageSetup.TopMargin = word_app.CentimetersToPoints(2);
            //Заголовок
            Word.Paragraph par_zag = doc.Content.Paragraphs.Add(ref oMissing);
            par_zag.Range.Text = "Отчёт о работе сотрудников";
            par_zag.Range.Font.Color = Word.WdColor.wdColorBlack;
            par_zag.Range.Font.Bold = 1;
            par_zag.Range.Font.Size = 16f;
            par_zag.Range.Font.Name = "Times New Roman";
            par_zag.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            par_zag.Range.InsertParagraphAfter();

            Word.Paragraph par_Date = doc.Content.Paragraphs.Add(ref oMissing);
            par_Date.Range.Text = "Дата: " + DateTime.Now;
            par_Date.Range.Font.Color = Word.WdColor.wdColorBlack;
            par_Date.Range.Font.Bold = 0;
            par_Date.Range.Font.Size = 14f;
            par_Date.Range.Font.Name = "Times New Roman";
            par_Date.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
            par_Date.Range.InsertParagraphAfter();

            Word.Paragraph par_number = doc.Content.Paragraphs.Add(ref oMissing);
            par_number.Range.Text = "Номер:01-20220618-0451";
            par_number.Range.Font.Color = Word.WdColor.wdColorBlack;
            par_number.Range.Font.Bold = 0;
            par_number.Range.Font.Size = 14f;
            par_number.Range.Font.Name = "Times New Roman";
            par_number.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
            par_number.Range.InsertParagraphAfter();

            Word.Paragraph par_smen = doc.Content.Paragraphs.Add(ref oMissing);
            par_smen.Range.Text = "Смена: Дневная";
            par_smen.Range.Font.Color = Word.WdColor.wdColorBlack;
            par_smen.Range.Font.Bold = 0;
            par_smen.Range.Font.Size = 14f;
            par_smen.Range.Font.Name = "Times New Roman";
            par_smen.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
            par_smen.Range.InsertParagraphAfter();

            Word.Paragraph par_admin = doc.Content.Paragraphs.Add(ref oMissing);
            par_admin.Range.Text = "Заведующий сменой: " + user.profile_name;
            par_admin.Range.Font.Color = Word.WdColor.wdColorBlack;
            par_admin.Range.Font.Bold = 0;
            par_admin.Range.Font.Size = 14f;
            par_admin.Range.Font.Name = "Times New Roman";
            par_admin.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
            par_admin.Range.InsertParagraphAfter();

            //Таблица
            Word.Paragraph table_par = doc.Content.Paragraphs.Add(ref oMissing);
            Word.Table table = doc.Content.Tables.Add(table_par.Range, DataBase.GetContext().User.Where(p => p.post == 2).Count() + 1, 14, ref oMissing, ref oMissing);
            table.Range.Font.Size = 10f;
            table.Range.Font.Bold = 0;
            table.Rows[1].Range.Font.Bold = 1;
            table.Cell(1, 1).Range.Text = "ФИО";
            table.Cell(1, 2).Range.Text = "8:00";
            table.Cell(1, 3).Range.Text = "9:00";
            table.Cell(1, 4).Range.Text = "10:00";
            table.Cell(1, 5).Range.Text = "11:00";
            table.Cell(1, 6).Range.Text = "12:00";
            table.Cell(1, 7).Range.Text = "13:00";
            table.Cell(1, 8).Range.Text = "14:00";
            table.Cell(1, 9).Range.Text = "15:00";
            table.Cell(1, 10).Range.Text = "16:00";
            table.Cell(1, 11).Range.Text = "17:00";
            table.Cell(1, 12).Range.Text = "18:00";
            table.Cell(1, 13).Range.Text = "19:00";
            table.Cell(1, 14).Range.Text = "20:00";
            table.Borders.InsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
            table.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
            for (int i = 0; i < DataBase.GetContext().User.Where(p => p.post == 2).Count(); i++)
            {
                for (int j = 1; j <= table.Columns.Count; j++)
                {
                    switch (j)
                    {
                        case 1:
                            table.Cell(i + 2, j).Range.Text = DataBase.GetContext().User.Where(p => p.post == 2).ToList()[i].profile_name;
                            break;
                        case 2:
                            table.Cell(i + 2, j).Range.Text = DataBase.GetContext().Response_to_the_appeal.Where(p =>  p.date > dateTime  && p.date < new DateTime(2022, 6, 18, 9, 0, 0) && p.id_user == i + 5).Count().ToString();
                            break;
                        case 3:
                            table.Cell(i + 2, j).Range.Text = DataBase.GetContext().Response_to_the_appeal.Where(p => p.date > new DateTime(2022, 6, 18, 9, 0, 0) && p.date < new DateTime(2022, 6, 18, 10, 0, 0) && p.id_user == i + 5).Count().ToString();
                            break;
                        case 4:
                            table.Cell(i + 2, j).Range.Text = DataBase.GetContext().Response_to_the_appeal.Where(p => p.date > new DateTime(2022, 6, 18, 10, 0, 0) && p.date < new DateTime(2022, 6, 18, 11, 0, 0) && p.id_user == i + 5).Count().ToString();
                            break;
                        case 5:
                            table.Cell(i + 2, j).Range.Text = DataBase.GetContext().Response_to_the_appeal.Where(p => p.date > new DateTime(2022, 6, 18, 11, 0, 0) && p.date < new DateTime(2022, 6, 18, 12, 0, 0) && p.id_user == i + 5).Count().ToString();
                            break;
                        case 6:
                            table.Cell(i + 2, j).Range.Text = DataBase.GetContext().Response_to_the_appeal.Where(p => p.date > new DateTime(2022, 6, 18, 12, 0, 0) && p.date < new DateTime(2022, 6, 18, 13, 0, 0) && p.id_user == i + 5).Count().ToString();
                            break;
                        case 7:
                            table.Cell(i + 2, j).Range.Text = DataBase.GetContext().Response_to_the_appeal.Where(p => p.date > new DateTime(2022, 6, 18, 13, 0, 0) && p.date < new DateTime(2022, 6, 18, 14, 0, 0) && p.id_user == i + 5).Count().ToString();
                            break;
                        case 8:
                            table.Cell(i + 2, j).Range.Text = DataBase.GetContext().Response_to_the_appeal.Where(p => p.date > new DateTime(2022, 6, 18, 14, 0, 0) && p.date < new DateTime(2022, 6, 18, 15, 0, 0) && p.id_user == i + 5).Count().ToString();
                            break;
                        case 9:
                            table.Cell(i + 2, j).Range.Text = DataBase.GetContext().Response_to_the_appeal.Where(p => p.date > new DateTime(2022, 6, 18, 15, 0, 0) && p.date < new DateTime(2022, 6, 18, 16, 0, 0) && p.id_user == i + 5).Count().ToString();
                            break;
                        case 10:
                            table.Cell(i + 2, j).Range.Text = DataBase.GetContext().Response_to_the_appeal.Where(p => p.date > new DateTime(2022, 6, 18, 16, 0, 0) && p.date < new DateTime(2022, 6, 18, 17, 0, 0) && p.id_user == i + 5).Count().ToString();
                            break;
                        case 11:
                            table.Cell(i + 2, j).Range.Text = DataBase.GetContext().Response_to_the_appeal.Where(p => p.date > new DateTime(2022, 6, 18, 17, 0, 0) && p.date < new DateTime(2022, 6, 18, 18, 0, 0) && p.id_user == i + 5).Count().ToString();
                            break;
                        case 12:
                            table.Cell(i + 2, j).Range.Text = DataBase.GetContext().Response_to_the_appeal.Where(p => p.date > new DateTime(2022, 6, 18, 18, 0, 0) && p.date < new DateTime(2022, 6, 18, 19, 0, 0) && p.id_user == i + 5).Count().ToString();
                            break;
                        case 13:
                            table.Cell(i + 2, j).Range.Text = DataBase.GetContext().Response_to_the_appeal.Where(p => p.date > new DateTime(2022, 6, 18, 19, 0, 0) && p.date < new DateTime(2022, 6, 18, 20, 0, 0) && p.id_user == i + 5).Count().ToString();
                            break;
                        case 14:
                            table.Cell(i + 2, j).Range.Text = DataBase.GetContext().Response_to_the_appeal.Where(p => p.date > new DateTime(2022, 6, 18, 20, 0, 0) && p.date < new DateTime(2022, 6, 18, 21, 0, 0) && p.id_user == i + 5).Count().ToString();
                            break;
                    }
                }
            }
            table_par.Range.InsertParagraphAfter();
            Word.Paragraph par_script = doc.Content.Paragraphs.Add(ref oMissing);
            par_script.Range.Text = "Подпись заведующего сменой_______________ Расшифровка_______________";
            par_script.Range.Font.Color = Word.WdColor.wdColorBlack;
            par_script.Range.Font.Bold = 0;
            par_script.Range.Font.Size = 14f;
            par_script.Range.Font.Name = "Times New Roman";
            par_script.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
            par_script.Range.InsertParagraphAfter();

            doc.SaveAs2(saveFileDialog.FileName = "Employee work Report", ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
            ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
            ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
        }

        private void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateData();
        }

        private void filterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateData();
        }

        private void CreateAppeal_Click(object sender, RoutedEventArgs e)
        {
            CreateAppealWindow createAppealWindow = new CreateAppealWindow(user);
            createAppealWindow.Owner = this;
            this.Hide();
            createAppealWindow.Show();
            
        }
    }
}
