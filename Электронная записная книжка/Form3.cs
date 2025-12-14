using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace Электронная_записная_книжка
{
    //Окно редактирования/добавления нового номера
    public partial class Form3 : Form
    {
        public Contact EditedContact { get; private set; }
        public event Action<Contact> OnContactSaved;
        private bool isNewContact;
        List<Contact> contacts2;

        public Form3(Contact contact = null, List<Contact> contacts = null)
        {
            InitializeComponent();
            this.MinimumSize = new Size(500, 650);
            isNewContact = (contact == null);
            EditedContact = new Contact();
            contacts2 = contacts;
            //Если это не новый контакт, то тогда вносим данные существующего контакта для дальнейшего редактирования
            if (!isNewContact)
            {
                // Копируем данные для редактирования
                EditedContact.ID = contact.ID;
                EditedContact.LastName = contact.LastName;
                EditedContact.FirstName = contact.FirstName;
                EditedContact.MiddleName = contact.MiddleName;
                EditedContact.Company = contact.Company;
                EditedContact.Position = contact.Position;
                EditedContact.Group = contact.Group;
                EditedContact.Important = contact.Important;
                EditedContact.Birthday = contact.Birthday;
                EditedContact.Address = contact.Address;
                EditedContact.Notes = contact.Notes;
                EditedContact.PhotoPath = contact.PhotoPath;
                EditedContact.Phones = new List<string>(contact.Phones);
                EditedContact.Emails = new List<string>(contact.Emails);

                LoadContactData();
            }
            else
            {
                this.Text = "Добавление нового контакта";
            }
        }
        //Выгрузка данных в форму
        private void LoadContactData()
        {
            // Загружаем ФИО в одно поле
            richTextBoxDisplayName.Text = EditedContact.DisplayName;

            // Остальные поля
            richTextBoxCompany.Text = EditedContact.Company ?? "";
            richTextBoxPosition.Text = EditedContact.Position ?? "";
            richTextBoxAddress.Text = EditedContact.Address ?? "";
            richTextBoxNotes.Text = EditedContact.Notes ?? "";
            comboBoxGroups.Text = EditedContact.Group ?? "";
            checkBoxImportant.Checked = EditedContact.Important.Equals(true);
            pictureBox1.Image = LoadContactPhoto(EditedContact);

            // День рождения
            if (EditedContact.Birthday.HasValue)
            {
                dateTimePickerBirthday.Value = EditedContact.Birthday.Value;
                dateTimePickerBirthday.Checked = true;
            }
            else
            {
                dateTimePickerBirthday.Checked = false;
            }


            // Телефоны
            if (EditedContact.Phones != null)
            {
                if (EditedContact.Phones.Count >= 1) richTextBoxPhone1.Text = EditedContact.Phones[0];
                if (EditedContact.Phones.Count >= 2) richTextBoxPhone2.Text = EditedContact.Phones[1];
                if (EditedContact.Phones.Count >= 3) richTextBoxPhone3.Text = EditedContact.Phones[2];
            }

            // Emails
            if (EditedContact.Emails != null)
            {
                if (EditedContact.Emails.Count >= 1) richTextBoxEmail1.Text = EditedContact.Emails[0];
                if (EditedContact.Emails.Count >= 2) richTextBoxEmail2.Text = EditedContact.Emails[1];
            }
        }
        //Загрузка изображения
        private Image LoadContactPhoto(Contact contact)
        {
            if (!string.IsNullOrEmpty(contact.PhotoPath) && File.Exists(contact.PhotoPath))
            {
                try
                {
                    Image original = Image.FromFile(contact.PhotoPath);
                    Image thumbnail = new Bitmap(original, new Size(170, 170));
                    original.Dispose();
                    return thumbnail;
                }
                catch
                {
                    return CreateDefaultAvatar(contact.DisplayName);
                }
            }
            return CreateDefaultAvatar(contact.DisplayName);
        }
        //Создание дефолтного изображения
        private Image CreateDefaultAvatar(string name)
        {
            Bitmap bmp = new Bitmap(170, 170);
            using (Graphics g = Graphics.FromImage(bmp))
            using (Font font = new Font("Arial", 60, FontStyle.Bold))
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
        //Событие при нажатии на кнопку "Сохранить"
        private void Save_Click(object sender, EventArgs e)
        {
            //Проверяем заполнение формы
            if (ValidateForm())
            {
                //Если всё верно, то сохраняем данные
                SaveChangesToContact();
                OnContactSaved?.Invoke(EditedContact);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
         //Загрзука изображения при нажатии на кнопку "Загрузить изображение"
        private void LoadPhoto_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Выберите фото контакта";
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Получаем выбранный файл
                        string selectedFilePath = openFileDialog.FileName;

                        // Загружаем изображение в PictureBox
                        Image photo = Image.FromFile(selectedFilePath);
                        pictureBox1.Image = photo;

                        // Сохраняем путь к фото в контакте
                        EditedContact.PhotoPath = selectedFilePath;

                        MessageBox.Show("Фото успешно загружено!", "Успех",
                                       MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка загрузки фото: {ex.Message}", "Ошибка",
                                       MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        //Метод проверки заполнения формы
        private bool ValidateForm()
        {
            // Проверка ФИО - должно быть 3 слова
            string fullName = richTextBoxDisplayName.Text.Trim();
            string[] nameParts = fullName.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (nameParts.Length != 3)
            {
                MessageBox.Show("ФИО должно состоять из 3 слов (Фамилия Имя Отчество)", "Ошибка",
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            //В случае отсутствия выбора группы автоматически будет устанавливаться "Другое"
            if (comboBoxGroups.Text=="")
            {
                comboBoxGroups.Text = "Другое";
            }

            // Проверка телефонов - хотя бы один номер
            List<string> phones = new List<string>();
            if (!string.IsNullOrWhiteSpace(richTextBoxPhone1.Text)) phones.Add(richTextBoxPhone1.Text);
            if (!string.IsNullOrWhiteSpace(richTextBoxPhone2.Text)) phones.Add(richTextBoxPhone2.Text);
            if (!string.IsNullOrWhiteSpace(richTextBoxPhone3.Text)) phones.Add(richTextBoxPhone3.Text);

            if (phones.Count == 0)
            {
                MessageBox.Show("Укажите хотя бы один номер телефона", "Ошибка",
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            // Проверка формата телефонов
            foreach (string phone in phones)
            {
                if (!IsValidPhone(phone))
                {
                    MessageBox.Show($"Некорректный формат телефона: {phone}\nТелефон должен начинаться с +7 и содержать 10 цифр",
                                   "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }

            // Проверка email - хотя бы один
            List<string> emails = new List<string>();
            if (!string.IsNullOrWhiteSpace(richTextBoxEmail1.Text)) emails.Add(richTextBoxEmail1.Text);
            if (!string.IsNullOrWhiteSpace(richTextBoxEmail2.Text)) emails.Add(richTextBoxEmail2.Text);

            if (emails.Count == 0)
            {
                MessageBox.Show("Укажите хотя бы один email", "Ошибка",
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Проверка формата emails
            foreach (string email in emails)
            {
                if (!IsValidEmail(email))
                {
                    MessageBox.Show($"Некорректный формат email: {email}", "Ошибка",
                                   MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }

            if ((checkBoxImportant.Checked) && (contacts2.Count(contact => contact.Important)==5))
            {
                MessageBox.Show($"У вас уже есть 5 контактов в избранных", "Ошибка",
                                   MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private bool IsValidPhone(string phone)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(phone, @"^\+7\d{10}$");
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        //Сохраняем данные
        private void SaveChangesToContact()
        {
            // Разделяем ФИО на отдельные поля
            string fullName = richTextBoxDisplayName.Text.Trim();
            string[] nameParts = fullName.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (nameParts.Length >= 1) EditedContact.LastName = nameParts[0];
            if (nameParts.Length >= 2) EditedContact.FirstName = nameParts[1];
            if (nameParts.Length >= 3) EditedContact.MiddleName = nameParts[2];

            // Остальные поля
            EditedContact.Company = richTextBoxCompany.Text.Trim();
            EditedContact.Position = richTextBoxPosition.Text.Trim();
            EditedContact.Address = richTextBoxAddress.Text.Trim();
            EditedContact.Notes = richTextBoxNotes.Text.Trim();
            EditedContact.Group = comboBoxGroups.Text.Trim();
            EditedContact.Important = checkBoxImportant.Checked;

            // День рождения
            if (dateTimePickerBirthday.Checked)
            {
                EditedContact.Birthday = dateTimePickerBirthday.Value.Date;
            }
            else
            {
                EditedContact.Birthday = null;
            }

            // Телефоны
            EditedContact.Phones = new List<string>();
            if (!string.IsNullOrWhiteSpace(richTextBoxPhone1.Text)) EditedContact.Phones.Add(richTextBoxPhone1.Text.Trim());
            if (!string.IsNullOrWhiteSpace(richTextBoxPhone2.Text)) EditedContact.Phones.Add(richTextBoxPhone2.Text.Trim());
            if (!string.IsNullOrWhiteSpace(richTextBoxPhone3.Text)) EditedContact.Phones.Add(richTextBoxPhone3.Text.Trim());

            // Emails
            EditedContact.Emails = new List<string>();
            if (!string.IsNullOrWhiteSpace(richTextBoxEmail1.Text)) EditedContact.Emails.Add(richTextBoxEmail1.Text.Trim());
            if (!string.IsNullOrWhiteSpace(richTextBoxEmail2.Text)) EditedContact.Emails.Add(richTextBoxEmail2.Text.Trim());
        }

    }
}
