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

namespace Электронная_записная_книжка
{
    //Окно подробного описания контакта
    public partial class Form2 : Form
    {
        private Contact contact;
        List<Contact> contacts1;

        // Добавляем события
        public event Action<Contact> OnContactUpdated;
        public event Action<int> OnContactDeleted; // Событие для удалени
        public Form2(Contact contact, List<Contact> contacts)
        {
            InitializeComponent();
            this.contact = contact;
            this.MinimumSize = new Size(500, 650);

            DisplayContactInfo(); //Отображение данных контакта в окне
            this.contacts1 = contacts;
        }
        private void DisplayContactInfo()
        {
            // Создаем словарь соответствия: RichTextBox -> значение
            var textBoxMap = new Dictionary<RichTextBox, Func<string>>
    {
        { richTextBoxDisplayName, () => contact.DisplayName ?? "" },
        { richTextBoxCompany, () => contact.Company ?? "" },
        { richTextBoxPosition, () => contact.Position ?? "" },
        { richTextBoxAddress, () => contact.Address ?? "" },
        { richTextBoxNotes, () => contact.Notes ?? "" },
        {richTextBoxGroups,() => contact.Group ?? "" }
        };
            //При наличии дня рождения оно отображается, иначе ставится сегодняшняя дата
            if (contact.Birthday.HasValue)
            {
                dateTimePickerBirthday.Value = contact.Birthday.Value;
            }
            else
            {
                dateTimePickerBirthday.Value = DateTime.Today;
            
            }
            //Если номер избранный, то будет появляться звездочка рядом с аватаркой
            if (contact.Important)
            {
                labelImportant.Text = "★";
            }

            // Заполняем все RichTextBox одним циклом
            foreach (var item in textBoxMap)
            {
                item.Key.Text = item.Value();
            }

            // Телефоны и emails отдельно
            FillMultipleTextBoxes(new[] { richTextBoxPhone1, richTextBoxPhone2, richTextBoxPhone3 }, contact.Phones);
            FillMultipleTextBoxes(new[] { richTextBoxEmail1, richTextBoxEmail2 }, contact.Emails);

            //Загрузка изображения
            pictureBox1.Image = LoadContactPhoto(contact);
            pictureBox1.Size = new Size(170, 170);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void FillMultipleTextBoxes(RichTextBox[] textBoxes, List<string> values)
        {
            for (int i = 0; i < textBoxes.Length; i++)
            {
                textBoxes[i].Text = (values != null && i < values.Count) ? values[i] : "";
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
        //Событие нажатия на кнопку "Изменить"
        private void Redact_Click(object sender, EventArgs e)
        {
            // Создаем форму редактирования и передаем текущий контакт
            Form3 editForm = new Form3(contact,contacts1);

            // Подписываемся на событие сохранения
            editForm.OnContactSaved += (updatedContact) =>
            {
                // Обновляем контакт в этой форме
                this.contact = updatedContact;

                // Обновляем отображение данных
                DisplayContactInfo();

                // Обновляем заголовок окна
                this.Text = $"Контакт: {contact.DisplayName}";

                // Уведомляем главную форму об изменениях
                UpdateContactInMainForm(updatedContact);
            };

            // Показываем форму редактирования
            editForm.ShowDialog();
        }
        //Обновление данных в основном окне
        private void UpdateContactInMainForm(Contact updatedContact)
        {
            // Вызываем метод в главной форме для обновления данных
            // Этот метод нужно создать в главной форме
            Form1 mainForm = Application.OpenForms["Form1"] as Form1;
            if (mainForm != null)
            {
                mainForm.UpdateContact(updatedContact);
            }
        }
        //Собтыие при нажатии на кнопку "Удалить"
        private void Delete_Click(object sender, EventArgs e)
        {
            // Подтверждение удаления
            DialogResult result = MessageBox.Show(
                $"Вы уверены, что хотите удалить контакт:\n{contact.DisplayName}?",
                "Подтверждение удаления",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                // Уведомляем главную форму об удалении
                OnContactDeleted?.Invoke(contact.ID);

                // Закрываем окно подробного описания
                this.Close();
            }
        }
    }
}
