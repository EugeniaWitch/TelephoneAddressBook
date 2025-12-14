using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics.Contracts;
using System.Runtime.Remoting.Contexts;

namespace Электронная_записная_книжка
{
    //Основное окно
    public partial class Form1 : Form
    {
        //Записываем файл в переменную
        public List<Contact> contacts = new List<Contact>();
        public string dataFilePath = "C:/foruni/contacts.txt";
        public Form1()
        {
            InitializeComponent();
            LoadContacts(); //Считывание данных из файла в list
            SaveContacts(); //Сохранение обратно в файл
            DisplayContactsInFlowLayoutPanel(); //Отображение контактов
            UpdateBirthdaysTextBox(); //Отображение ближайших дней рождений
            toolStripComboBox1.SelectedIndex = 0; //Задаем по умолчанию "Без фильтров"
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged; //Считывание выбранной группы
            QuickSearch.TextChanged += QuickSearch_TextChanged; //Считывание текста в поиске
            toolStripComboBox1.SelectedIndexChanged += Filter_SelectedIndexChanged; //Считывание фильтра
            ConfigureGroupsListBox(); //Отображение групп
            FilterAndDisplayContacts(); //Отображение контактов с учетом фильтров, поиска и группы
        }
        //Читаем данные, полученные из файла, и создаем объекты 
        private void LoadContacts()
        {
            try
            {
                contacts.Clear();

                if (!File.Exists(dataFilePath))
                {
                    // Создаем файл с заголовками
                    File.WriteAllText(dataFilePath, "ID|Important|PhotoPath|LastName|FirstName|MiddleName|Birthday|Group|Company|Position|Phones|Emails|Address|Notes");
                    return;
                }

                var lines = File.ReadAllLines(dataFilePath);
                if (lines.Length <= 1) return;

                // Пропускаем заголовок
                for (int i = 1; i < lines.Length; i++)
                {
                    var line = lines[i];
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    var parts = line.Split('|');
                    if (parts.Length >= 13)
                    {
                        var contact = new Contact
                        {
                            ID = int.Parse(parts[0]),
                            Important = bool.Parse(parts[1]),
                            PhotoPath = parts[2],
                            LastName = parts[3],
                            FirstName = parts[4],
                            MiddleName = parts[5],
                            Group = parts[7],
                            Company = parts[8],
                            Position = parts[9],
                            Address = parts[12],
                            Notes = parts[13]
                        };

                        // Обработка телефонов (разделитель - ;)
                        if (!string.IsNullOrEmpty(parts[10]))
                            contact.Phones = parts[10].Split(';').ToList();

                        // Обработка email (разделитель - ;)
                        if (!string.IsNullOrEmpty(parts[11]))
                            contact.Emails = parts[11].Split(';').ToList();

                        // Обработка даты рождения
                        if (!string.IsNullOrEmpty(parts[6]) && DateTime.TryParse(parts[6], out DateTime bday))
                            contact.Birthday = bday;

                        contacts.Add(contact);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки: {ex.Message}", "Ошибка",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                contacts.Clear();
            }
        }
        //Сохранение данных обратно в файл
        private void SaveContacts()
        {
            var lines = new List<string>
        {
            "ID|Important|PhotoPath|LastName|FirstName|MiddleName|Birthday|Group|Company|Position|Phones|Emails|Address|Notes"
        };

            foreach (var contact in contacts)
            {
                var line = $"{contact.ID}|{contact.Important}|{contact.PhotoPath}|{contact.LastName}|{contact.FirstName}|" +
                          $"{contact.MiddleName}|{contact.Birthday?.ToString("dd.MM.yyyy")}|{contact.Group}|{contact.Company}|" +
                          $"{contact.Position}|{string.Join(";", contact.Phones)}|{string.Join(";", contact.Emails)}|" +
                          $"{contact.Address}|{contact.Notes}";
                lines.Add(line);
            }

            File.WriteAllLines(dataFilePath, lines);
        }
        //Метод сортировки, чтобы при отображении контактов избранные номера всегда были наверху
        private List<Contact> SortContactsWithImportantFirst(List<Contact> contactsToSort)
        {
            // Сначала избранные, потом обычные
            var importantContacts = contactsToSort.Where(c => c.Important);
            var normalContacts = contactsToSort.Where(c => !c.Important);
            // Сортируем каждую группу по имени
            var sortedImportant = importantContacts
                .OrderBy(c => c.LastName)
                .ThenBy(c => c.FirstName)
                .ThenBy(c => c.MiddleName);

            var sortedNormal = normalContacts
                .OrderBy(c => c.LastName)
                .ThenBy(c => c.FirstName)
                .ThenBy(c => c.MiddleName);

            // Объединяем: сначала избранные, потом обычные
            return sortedImportant.Concat(sortedNormal).ToList();
        }
        //Отображение контактов
        private void DisplayContactsInFlowLayoutPanel()
        {
            // Очищаем FlowLayoutPanel
            flowLayoutPanel1.Controls.Clear();
            //Сортируем, чтобы наверху были избранные номера
            var sortedContacts = SortContactsWithImportantFirst(contacts);

            // Добавляем каждый контакт в FlowLayoutPanel
            foreach (var contact in sortedContacts)
            {
                // Создаем панель для контакта
                Panel contactPanel = CreateContactPanel(contact);

                // Добавляем панель в FlowLayoutPanel
                flowLayoutPanel1.Controls.Add(contactPanel);
            }
        }
        //Создание панели контакта
        private Panel CreateContactPanel(Contact contact)
        {
            // Создаем основную панель контакта
            Panel panel = new Panel();
            panel.Size = new Size(flowLayoutPanel1.Width - 25, 60); // Высота 60px, ширина почти на всю панель
            panel.BackColor = contact.Important ? Color.LightYellow : Color.White; //Делаем фон избранных номеров слегка другим цветом
            panel.BorderStyle = BorderStyle.FixedSingle;
            panel.Margin = new Padding(3);
            panel.Tag = contact.ID; // Сохраняем ID для идентификации
            panel.Cursor = Cursors.Hand;

            // Добавляем обработчик клика
            panel.Click += (s, e) => ShowContactDetails(contact);

            // PictureBox для фото
            PictureBox picPhoto = new PictureBox();
            picPhoto.Size = new Size(50, 50);
            picPhoto.Location = new Point(5, 5);
            picPhoto.SizeMode = PictureBoxSizeMode.Zoom;
            picPhoto.Image = LoadContactPhoto(contact);

            // Label для имени
            Label lblName = new Label();
            string namePrefix = contact.Important ? "★ " : ""; // Добавляем звездочку для избранных
            lblName.Text = namePrefix + contact.DisplayName;
            lblName.Location = new Point(60, 10);
            lblName.Size = new Size(panel.Width - 70, 20);
            lblName.Font = new Font("Segoe UI", 10, contact.Important ? FontStyle.Bold : FontStyle.Regular);
            lblName.Click += (s, e) => ShowContactDetails(contact);

            // Label для телефона
            Label lblPhone = new Label();
            lblPhone.Text = contact.PrimaryPhone + " | " + contact.Birthday?.ToString("dd.MM.yyyy") ?? "";
            lblPhone.Location = new Point(60, 30);
            lblPhone.Size = new Size(panel.Width - 70, 20);
            lblPhone.Font = new Font("Segoe UI", 9);
            lblPhone.ForeColor = Color.Gray;
            lblPhone.Click += (s, e) => ShowContactDetails(contact);

            // Добавляем элементы на панель
            panel.Controls.Add(picPhoto);
            panel.Controls.Add(lblName);
            panel.Controls.Add(lblPhone);

            return panel;
        }
        //Метод загрузки аватарки контакта
        private Image LoadContactPhoto(Contact contact)
        {
            if (!string.IsNullOrEmpty(contact.PhotoPath) && File.Exists(contact.PhotoPath))
            {
                try
                {
                    Image original = Image.FromFile(contact.PhotoPath);
                    Image thumbnail = new Bitmap(original, new Size(50, 50));
                    original.Dispose();
                    return thumbnail;
                }
                catch
                {
                    //В случае некорректного пути на фото создается дефолтное изображение
                    return CreateDefaultAvatar(contact.DisplayName);
                }
            }
            //В случае отсутствия пути для фото создается дефолтное изображение
            return CreateDefaultAvatar(contact.DisplayName);
        }
        //Создание дефолтного изображение
        private Image CreateDefaultAvatar(string name)
        {
            //Создаем изображение с первой буквой фамилии
            Bitmap bmp = new Bitmap(50, 50);
            using (Graphics g = Graphics.FromImage(bmp))
            using (Font font = new Font("Arial", 14, FontStyle.Bold))
            {
                g.Clear(Color.Gray);

                string firstLetter = name.Length > 0 ? name[0].ToString().ToUpper() : "?";

                StringFormat format = new StringFormat();
                format.Alignment = StringAlignment.Center;
                format.LineAlignment = StringAlignment.Center;

                g.DrawString(firstLetter, font, Brushes.White,
                            new RectangleF(0, 0, bmp.Width, bmp.Height), format);
            }
            return bmp;
        }
        //Открытие формы с подробным описание номера при нажатии на панель контакта
        private void ShowContactDetails(Contact contact)
        {
            Form2 detailsForm = new Form2(contact,contacts);

            // Подписываемся на обновление контакта
            detailsForm.OnContactUpdated += (updatedContact) =>
            {
                var index = contacts.FindIndex(c => c.ID == updatedContact.ID);
                if (index >= 0)
                {
                    contacts[index] = updatedContact;
                    SaveContacts();
                    FilterAndDisplayContacts();
                    ConfigureGroupsListBox();
                    UpdateBirthdaysTextBox();

                    MessageBox.Show("Контакт успешно обновлен!", "Успех",
                                   MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            };

            // Подписываемся на удаление контакта
            detailsForm.OnContactDeleted += (contactId) =>
            {
                // Удаляем контакт из списка
                contacts.RemoveAll(c => c.ID == contactId);

                // Сохраняем изменения в файл
                SaveContacts();

                // Обновляем отображение
                FilterAndDisplayContacts();
                ConfigureGroupsListBox();
                UpdateBirthdaysTextBox();

                MessageBox.Show("Контакт успешно удален!", "Успех",
                               MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            detailsForm.ShowDialog();
        }
        //Отображение ближайших дней рождений (30 дней)
        private void UpdateBirthdaysTextBox()
        {
            richTextBox1.Clear();
            StringBuilder sb = new StringBuilder();
            DateTime today = DateTime.Today;
            var birthdaysInNext30Days = new List<(Contact contact, int daysUntil)>();

            foreach (var contact in contacts)
            {
                if (!contact.Birthday.HasValue) continue;

                DateTime birthday = contact.Birthday.Value;
                DateTime nextBirthday = new DateTime(today.Year, birthday.Month, birthday.Day);

                if (nextBirthday < today)
                {
                    nextBirthday = nextBirthday.AddYears(1);
                }

                int daysUntil = (nextBirthday - today).Days;

                if (daysUntil <= 30)
                {
                    birthdaysInNext30Days.Add((contact, daysUntil));
                }
            }

            if (birthdaysInNext30Days.Count == 0)
            {
                richTextBox1.Text = "В ближайшие 30 дней дней рождений нет";
                return;
            }

            var sortedBirthdays = birthdaysInNext30Days.OrderBy(b => b.daysUntil).ToList();

            foreach (var birthday in sortedBirthdays)
            {
                string daysText;
                switch (birthday.daysUntil)
                {
                    case 0:
                        daysText = "сегодня";
                        break;
                    case 1:
                        daysText = "завтра";
                        break;
                    default:
                        daysText = $"через {birthday.daysUntil} дн.";
                        break;
                }

                sb.AppendLine($" • {birthday.contact.LastName} {birthday.contact.FirstName} ({daysText})");
            }

            richTextBox1.Text = sb.ToString();
        }
        //Сортировка контактов
        private List<Contact> SortContactsWithImportantFirstSearch(List<Contact> contactsToSort)
        {
            if (contactsToSort == null || contactsToSort.Count == 0)
                return contactsToSort;

            // Разделяем на избранные и обычные
            var importantContacts = contactsToSort.Where(c => c.Important).ToList();
            var normalContacts = contactsToSort.Where(c => !c.Important).ToList();

            // Сортируем КАЖДУЮ группу пузырьковой сортировкой по ФИО
            var sortedImportant = BubbleSortContacts(importantContacts);
            var sortedNormal = BubbleSortContacts(normalContacts);

            // Объединяем: сначала избранные, потом обычные
            return sortedImportant.Concat(sortedNormal).ToList();
        }
        //Сортировка методом пузырька
        private List<Contact> BubbleSortContacts(List<Contact> contactsToSort)
        {
            if (contactsToSort == null || contactsToSort.Count <= 1)
                return contactsToSort;

            var sorted = new List<Contact>(contactsToSort);
            int n = sorted.Count;
            bool swapped;

            do
            {
                swapped = false;
                for (int i = 0; i < n - 1; i++)
                {
                    // Сравниваем ФИО
                    string name1 = sorted[i].DisplayName;
                    string name2 = sorted[i + 1].DisplayName;

                    if (string.Compare(name1, name2, StringComparison.OrdinalIgnoreCase) > 0)
                    {
                        // Меняем местами
                        var temp = sorted[i];
                        sorted[i] = sorted[i + 1];
                        sorted[i + 1] = temp;
                        swapped = true;
                    }
                }
                n--;
            } while (swapped);

            return sorted;
        }
        private void DisplayContacts(List<Contact> contactsToDisplay)
        {
            flowLayoutPanel1.Controls.Clear();

            if (contactsToDisplay.Count == 0)
            {
                Label noResults = new Label();
                noResults.Text = "Контакты не найдены";
                noResults.Font = new Font("Segoe UI", 11, FontStyle.Italic);
                noResults.ForeColor = Color.Gray;
                noResults.Size = new Size(flowLayoutPanel1.Width - 25, 40);
                noResults.TextAlign = ContentAlignment.MiddleCenter;
                flowLayoutPanel1.Controls.Add(noResults);
            }
            else
            {
                foreach (var contact in contactsToDisplay)
                {
                    Panel contactPanel = CreateContactPanel(contact);
                    flowLayoutPanel1.Controls.Add(contactPanel);
                }
            }
        }
        // Сортировка по фамилии
        private List<Contact> BubbleSortByLastName(List<Contact> contactsToSort, bool ascending)
        {
            if (contactsToSort == null || contactsToSort.Count <= 1)
                return contactsToSort;

            var sorted = new List<Contact>(contactsToSort);
            int n = sorted.Count;

            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - i - 1; j++)
                {
                    string lastName1 = sorted[j].LastName ?? "";
                    string lastName2 = sorted[j + 1].LastName ?? "";

                    int comparison = string.Compare(lastName1, lastName2, StringComparison.OrdinalIgnoreCase);

                    // Для ascending: если текущий больше следующего - меняем местами
                    // Для descending: если текущий меньше следующего - меняем местами
                    bool needSwap = ascending ? comparison > 0 : comparison < 0;

                    if (needSwap)
                    {
                        // Меняем местами
                        Contact temp = sorted[j];
                        sorted[j] = sorted[j + 1];
                        sorted[j + 1] = temp;
                    }
                }
            }

            return sorted;
        }

        // Сортировка по телефону
        private List<Contact> BubbleSortByPhone(List<Contact> contactsToSort, bool ascending)
        {
            if (contactsToSort == null || contactsToSort.Count <= 1)
                return contactsToSort;

            var sorted = new List<Contact>(contactsToSort);
            int n = sorted.Count;

            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - i - 1; j++)
                {
                    string phone1 = sorted[j].Phones.Count > 0 ? sorted[j].Phones[0] : "";
                    string phone2 = sorted[j + 1].Phones.Count > 0 ? sorted[j + 1].Phones[0] : "";

                    // Очищаем от нецифровых символов
                    string cleanPhone1 = new string(phone1.Where(char.IsDigit).ToArray());
                    string cleanPhone2 = new string(phone2.Where(char.IsDigit).ToArray());

                    // Заполняем нулями до одинаковой длины для корректного сравнения
                    int maxLength = Math.Max(cleanPhone1.Length, cleanPhone2.Length);
                    cleanPhone1 = cleanPhone1.PadLeft(maxLength, '0');
                    cleanPhone2 = cleanPhone2.PadLeft(maxLength, '0');

                    int comparison = string.Compare(cleanPhone1, cleanPhone2, StringComparison.Ordinal);

                    bool needSwap = ascending ? comparison > 0 : comparison < 0;

                    if (needSwap)
                    {
                        Contact temp = sorted[j];
                        sorted[j] = sorted[j + 1];
                        sorted[j + 1] = temp;
                    }
                }
            }

            return sorted;
        }

        // Сортировка по дню рождения (ближайшие/дальние)
        private List<Contact> BubbleSortByBirthday(List<Contact> contactsToSort, bool ascending)
        {
            if (contactsToSort == null || contactsToSort.Count <= 1)
                return contactsToSort;

            var sorted = new List<Contact>(contactsToSort);
            int n = sorted.Count;

            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - i - 1; j++)
                {
                    int days1 = GetDaysUntilNextBirthday(sorted[j].Birthday);
                    int days2 = GetDaysUntilNextBirthday(sorted[j + 1].Birthday);

                    // Для ascending (ближайшие): меньшие дни вперед
                    // Для descending (дальние): большие дни вперед
                    bool needSwap = ascending ? days1 > days2 : days1 < days2;

                    if (needSwap)
                    {
                        Contact temp = sorted[j];
                        sorted[j] = sorted[j + 1];
                        sorted[j + 1] = temp;
                    }
                }
            }

            return sorted;
        }

        private int GetDaysUntilNextBirthday(DateTime? birthday)
        {
            if (!birthday.HasValue) return 9999; // Контакты без ДР в конец

            DateTime today = DateTime.Today;
            DateTime nextBirthday = new DateTime(today.Year, birthday.Value.Month, birthday.Value.Day);

            if (nextBirthday < today)
            {
                nextBirthday = nextBirthday.AddYears(1);
            }

            return (nextBirthday - today).Days;
        }

        // Сортировка по месту работы
        private List<Contact> BubbleSortByCompany(List<Contact> contactsToSort, bool ascending)
        {
            if (contactsToSort == null || contactsToSort.Count <= 1)
                return contactsToSort;

            var sorted = new List<Contact>(contactsToSort);
            int n = sorted.Count;

            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - i - 1; j++)
                {
                    string company1 = sorted[j].Company ?? "";
                    string company2 = sorted[j + 1].Company ?? "";

                    int comparison = string.Compare(company1, company2, StringComparison.OrdinalIgnoreCase);

                    bool needSwap = ascending ? comparison > 0 : comparison < 0;

                    if (needSwap)
                    {
                        Contact temp = sorted[j];
                        sorted[j] = sorted[j + 1];
                        sorted[j + 1] = temp;
                    }
                }
            }

            return sorted;
        }

        // Сортировка по email
        private List<Contact> BubbleSortByEmail(List<Contact> contactsToSort, bool ascending)
        {
            if (contactsToSort == null || contactsToSort.Count <= 1)
                return contactsToSort;

            var sorted = new List<Contact>(contactsToSort);
            int n = sorted.Count;

            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - i - 1; j++)
                {
                    string email1 = sorted[j].Emails.Count > 0 ? sorted[j].Emails[0] : "";
                    string email2 = sorted[j + 1].Emails.Count > 0 ? sorted[j + 1].Emails[0] : "";

                    int comparison = string.Compare(email1, email2, StringComparison.OrdinalIgnoreCase);

                    bool needSwap = ascending ? comparison > 0 : comparison < 0;

                    if (needSwap)
                    {
                        Contact temp = sorted[j];
                        sorted[j] = sorted[j + 1];
                        sorted[j + 1] = temp;
                    }
                }
            }

            return sorted;
        }

        // Сортировка по адресу
        private List<Contact> BubbleSortByAddress(List<Contact> contactsToSort, bool ascending)
        {
            if (contactsToSort == null || contactsToSort.Count <= 1)
                return contactsToSort;

            var sorted = new List<Contact>(contactsToSort);
            int n = sorted.Count;

            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - i - 1; j++)
                {
                    string address1 = sorted[j].Address ?? "";
                    string address2 = sorted[j + 1].Address ?? "";

                    int comparison = string.Compare(address1, address2, StringComparison.OrdinalIgnoreCase);

                    bool needSwap = ascending ? comparison > 0 : comparison < 0;

                    if (needSwap)
                    {
                        Contact temp = sorted[j];
                        sorted[j] = sorted[j + 1];
                        sorted[j + 1] = temp;
                    }
                }
            }
            return sorted;
        }
        //Отображение групп с учетом количества контактов в каждой группе
        private void ConfigureGroupsListBox()
        {
            listBox1.Items.Clear();

            int allCount = contacts.Count;
            int familyCount = contacts.Count(c => c.Group == "Семья");
            int workCount = contacts.Count(c => c.Group == "Работа");
            int friendsCount = contacts.Count(c => c.Group == "Друзья");
            int studyCount = contacts.Count(c => c.Group == "Учеба");
            int relativesCount = contacts.Count(c => c.Group == "Родственники");
            int otherCount = contacts.Count(c => c.Group == "Другое");

            listBox1.Items.Add($"Все контакты ({allCount})");
            listBox1.Items.Add($"Семья ({familyCount})");
            listBox1.Items.Add($"Друзья ({friendsCount})");
            listBox1.Items.Add($"Учеба ({studyCount})");
            listBox1.Items.Add($"Работа ({workCount})");
            listBox1.Items.Add($"Родственники ({relativesCount})");
            listBox1.Items.Add($"Другое ({otherCount})");
        }
        //Отображение с учетом фильтрации, поиска и выбранной группы
        private void FilterAndDisplayContacts()
        {
            // Очищаем панель
            flowLayoutPanel1.Controls.Clear();

            // 1. Получаем текущую выбранную группу
            string currentGroup = "Все контакты";
            if (listBox1.SelectedItem != null)
            {
                string selectedText = listBox1.SelectedItem.ToString();
                // Извлекаем чистое название группы из текста "Группа (10)"
                if (selectedText.Contains("("))
                {
                    currentGroup = selectedText.Substring(0, selectedText.IndexOf("(")).Trim();
                }
                else
                {
                    currentGroup = selectedText;
                }
            }

            // 2. Фильтруем контакты по выбранной группе
            List<Contact> filteredContacts;
            switch (currentGroup)
            {
                case "Все контакты":
                    filteredContacts = contacts;
                    break;
                case "Работа":
                    filteredContacts = contacts.Where(c => c.Group == "Работа").ToList();
                    break;
                case "Друзья":
                    filteredContacts = contacts.Where(c => c.Group == "Друзья").ToList();
                    break;
                case "Семья":
                    filteredContacts = contacts.Where(c => c.Group == "Семья").ToList();
                    break;
                case "Учеба":
                    filteredContacts = contacts.Where(c => c.Group == "Учеба").ToList();
                    break;
                case "Другое":
                    filteredContacts = contacts.Where(c => c.Group == "Другое").ToList();
                    break;
                case "Родственники":
                    filteredContacts = contacts.Where(c => c.Group == "Родственники").ToList();
                    break;
                default:
                    filteredContacts = contacts;
                    break;
            }

            // 3. Применяем поиск, если он есть
            string searchText = QuickSearch.Text.Trim();
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                searchText = searchText.ToLower();
                var foundContacts = new List<Contact>();

                foreach (var contact in filteredContacts)
                {
                    bool found = false;

                    // Поиск по ФИО
                    if (contact.DisplayName.ToLower().Contains(searchText))
                    {
                        found = true;
                    }
                    // Поиск по телефонам
                    else if (contact.Phones != null)
                    {
                        foreach (var phone in contact.Phones)
                        {
                            if (phone.ToLower().Contains(searchText))
                            {
                                found = true;
                                break;
                            }
                        }
                    }
                    // Поиск по дате рождения
                    else if (contact.Birthday.HasValue)
                    {
                        string[] dateFormats = {
                    contact.Birthday.Value.ToString("dd.MM.yyyy"),
                    contact.Birthday.Value.ToString("dd.MM.yy"),
                    contact.Birthday.Value.ToString("d.M.yyyy"),
                    contact.Birthday.Value.ToString("dd"),
                    contact.Birthday.Value.ToString("MM"),
                    contact.Birthday.Value.ToString("M"),
                    contact.Birthday.Value.ToString("yyyy")
                };

                        foreach (string dateStr in dateFormats)
                        {
                            if (dateStr.ToLower().Contains(searchText))
                            {
                                found = true;
                                break;
                            }
                        }
                    }

                    if (found)
                    {
                        foundContacts.Add(contact);
                    }
                }
                filteredContacts = foundContacts;
            }

            // 4. Применяем фильтрацию
            string currentFilter = toolStripComboBox1?.SelectedItem?.ToString() ?? "Без фильтров";
            List<Contact> sortedContacts;

            switch (currentFilter)
            {
                case "Без фильтров":
                    sortedContacts = SortContactsWithImportantFirst(filteredContacts);
                    break;
                case "Фамилия (от А до Я)":
                    sortedContacts = BubbleSortByLastName(filteredContacts, true);
                    break;
                case "Фамилия (от Я до А)":
                    sortedContacts = BubbleSortByLastName(filteredContacts, false);
                    break;
                case "Телефон (от 0 до 9)":
                    sortedContacts = BubbleSortByPhone(filteredContacts, true);
                    break;
                case "Телефон (от 9 до 0)":
                    sortedContacts = BubbleSortByPhone(filteredContacts, false);
                    break;
                case "День рождения (ближайшие)":
                    sortedContacts = BubbleSortByBirthday(filteredContacts, true);
                    break;
                case "День рождения (дальние)":
                    sortedContacts = BubbleSortByBirthday(filteredContacts, false);
                    break;
                case "Место работы (от А до Я)":
                    sortedContacts = BubbleSortByCompany(filteredContacts, true);
                    break;
                case "Место работы (от Я до А)":
                    sortedContacts = BubbleSortByCompany(filteredContacts, false);
                    break;
                case "Email (от A до Z)":
                    sortedContacts = BubbleSortByEmail(filteredContacts, true);
                    break;
                case "Email (от Z до A)":
                    sortedContacts = BubbleSortByEmail(filteredContacts, false);
                    break;
                case "Адрес (от А до Я)":
                    sortedContacts = BubbleSortByAddress(filteredContacts, true);
                    break;
                case "Адрес (от Я до А)":
                    sortedContacts = BubbleSortByAddress(filteredContacts, false);
                    break;
                default:
                    sortedContacts = filteredContacts;
                    break;
            }

            // 5. Отображаем результаты
            if (sortedContacts.Count == 0)
            {
                Label noResults = new Label();
                noResults.Text = "Контакты не найдены";
                noResults.Font = new Font("Segoe UI", 11, FontStyle.Italic);
                noResults.ForeColor = Color.Gray;
                noResults.Size = new Size(flowLayoutPanel1.Width - 25, 40);
                noResults.TextAlign = ContentAlignment.MiddleCenter;
                flowLayoutPanel1.Controls.Add(noResults);
            }
            else
            {
                foreach (var contact in sortedContacts)
                {
                    Panel contactPanel = CreateContactPanel(contact);
                    flowLayoutPanel1.Controls.Add(contactPanel);
                }
            }

            
        }
        //При изменении группы срабатывает метод, меняющий отображение контактов
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterAndDisplayContacts();
        }
        //При изменении текста в поиске срабатывает метод, меняющий отображение контактов
        private void QuickSearch_TextChanged(object sender, EventArgs e)
        {
            FilterAndDisplayContacts();
        }
        //При изменении фильтра срабатывает метоД. меняющий отображение контактов
        private void Filter_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterAndDisplayContacts();
        }
        //Событие при клике на кнопку созданя нового контакта
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            AddNewContact();
        }
        //Открывает окно редактирования/добавления и записывает данные
        private void AddNewContact()
        {
            Form3 editForm = new Form3(null,contacts); // Без параметра = новый контакт
            editForm.OnContactSaved += (savedContact) =>
            {
                // Для нового контакта генерируем ID
                if (savedContact.ID == 0)
                {
                    savedContact.ID = GetNextContactId();
                }

                contacts.Add(savedContact);
                SaveContacts();
                FilterAndDisplayContacts();
                ConfigureGroupsListBox();
                UpdateBirthdaysTextBox();

                MessageBox.Show("Контакт успешно добавлен!", "Успех",
                               MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            editForm.ShowDialog();
        }
        //Генерация ID
        private int GetNextContactId()
        {
            if (contacts.Count == 0) return 1;
            return contacts.Max(c => c.ID) + 1;
        }
        //Обновление существующего контакта
        public void UpdateContact(Contact updatedContact)
        {
            // Находим контакт в списке по ID
            var existingContact = contacts.FirstOrDefault(c => c.ID == updatedContact.ID);
            if (existingContact != null)
            {
                // Обновляем данные контакта
                existingContact.LastName = updatedContact.LastName;
                existingContact.FirstName = updatedContact.FirstName;
                existingContact.MiddleName = updatedContact.MiddleName;
                existingContact.Company = updatedContact.Company;
                existingContact.Position = updatedContact.Position;
                existingContact.Group = updatedContact.Group;
                existingContact.Important = updatedContact.Important;
                existingContact.Birthday = updatedContact.Birthday;
                existingContact.Address = updatedContact.Address;
                existingContact.Notes = updatedContact.Notes;
                existingContact.PhotoPath = updatedContact.PhotoPath;
                existingContact.Phones = new List<string>(updatedContact.Phones);
                existingContact.Emails = new List<string>(updatedContact.Emails);

                // Сохраняем изменения в файл
                SaveContacts();

                // Обновляем отображение в главном окне
                FilterAndDisplayContacts();
                ConfigureGroupsListBox();
                UpdateBirthdaysTextBox();
            }
        }
        //Удаление контакта из списка
        private void DeleteContactFromList(int contactId)
        {
            // Находим и удаляем контакт из списка
            var contactToDelete = contacts.FirstOrDefault(c => c.ID == contactId);
            if (contactToDelete != null)
            {
                contacts.Remove(contactToDelete);
                SaveContacts();
                FilterAndDisplayContacts();
                ConfigureGroupsListBox();
                UpdateBirthdaysTextBox();

                MessageBox.Show("Контакт успешно удален!", "Успех",
                               MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

       
    }
    //Класс, в котором хранятся все атрибуты контактов
    public class Contact
    {
            public int ID { get; set; }
            public bool Important { get; set; }
            public string PhotoPath { get; set; }
            public string LastName { get; set; }
            public string FirstName { get; set; }
            public string MiddleName { get; set; }
            public DateTime? Birthday { get; set; }
            public string Group { get; set; }
            public string Company { get; set; }
            public string Position { get; set; }
            public List<string> Phones { get; set; } = new List<string>();
            public List<string> Emails { get; set; } = new List<string>();
            public string Address { get; set; }
            public string Notes { get; set; }
            public string DisplayName => $"{LastName} {FirstName} {MiddleName}".Trim();
            public string PrimaryPhone => Phones.Count > 0 ? Phones[0] : "";
            public string PrimaryEmail => Emails.Count > 0 ? Emails[0] : "";
    }  
}
