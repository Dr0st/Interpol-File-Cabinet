﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Interpol_file_cabinet.Model
{
    static class MethodsForMainForm
    {
        /// <summary>
        /// Изменяет строку в DataGridView после редактирования
        /// </summary>
        /// <param name="DGV">DataGridView в котором нужно осуществить перезапись</param>
        /// <param name="crim">Уже отредактированный преступник</param>
        public static void ChangeRowInForm(DataGridView DGV, Criminal crim)
        {
            // Найти индекс преступника, который был отредактирован
            int ind = MyCollection.criminals.FindIndex(key =>
                key == ActionsWithFields.ConvertToCriminal(DGV.CurrentRow));
            MyCollection.criminals.RemoveAt(ind);

            DGV.CurrentRow.Cells[0].Value = crim.Surname;
            DGV.CurrentRow.Cells[1].Value = crim.Name;
            DGV.CurrentRow.Cells[2].Value = crim.Patronymic;
            DGV.CurrentRow.Cells[3].Value = crim.Nickname;
            DGV.CurrentRow.Cells[4].Value = crim.PlaceOfBirth;
            DGV.CurrentRow.Cells[5].Value = crim.DateOfBirth;
            DGV.CurrentRow.Cells[6].Value = crim.Height;
            DGV.CurrentRow.Cells[7].Value = crim.Weight;
            DGV.CurrentRow.Cells[8].Value = crim.EyeColor;
            DGV.CurrentRow.Cells[9].Value = crim.SpecialSigns;
            DGV.CurrentRow.Cells[10].Value = crim.Profession;

            crim.Group = DGV.CurrentRow.Cells[11].Value.ToString();
            // Добавить уже отредактированного преступника
            MyCollection.criminals.Insert(ind, crim);
        }

        /// <summary>
        /// Переместить преступника с главной панели
        /// </summary>
        /// <param name="DGV">DataGridView в который добавить преступника</param>
        /// <param name="list">Новое место расположения преступника</param>
        /// <param name="numOfDiv">Номер пнаели, куда поместить преступника</param>
        public static void MoveCriminalFromMainTable(DataGridView DGV, List<Criminal> list, int numOfDiv)
        {
            if (DGV.CurrentCell == null)
            {
                MessageBox.Show("Выберите преступника", "Ошибка");
                return;
            }

            MainForm MForm = new MainForm();
            Criminal newCrim = MyCollection.criminals.Find(key => key == ActionsWithFields.ConvertToCriminal(DGV.CurrentRow));

            // Удалить перступника с главной панели
            MyCollection.criminals.Remove(newCrim);

            // Если преступник состоит в группировке, удалить его и оттуда(уменьшить кол-во членов)
            if (newCrim.Group != "")
            {
                for (int i = 0; i < MyCollection.groups.Count; i++)
                {
                    if (MyCollection.groups[i].Name == newCrim.Group)
                    {
                        MyCollection.groups[i].CountOfCriminals--;
                        break;
                    }
                }

                //Уменьшить кол-во преступников в группировке на 1
                foreach (DataGridViewRow row in DGV.Rows)
                {
                    if (row.Cells[0].Value.ToString() == newCrim.Group)
                    {
                        row.Cells[1].Value = (Convert.ToInt32(row.Cells[1].Value) - 1).ToString();
                        break;
                    }
                }
            }

            newCrim.Group = "";
            list.Add(newCrim);
            MForm.AddCriminal(newCrim, numOfDiv, false, false);
            DGV.Rows.Remove(DGV.CurrentRow);

            ActionsWithFields.wasChangedData = true;
        }

        /// <summary>
        /// Изменяет кол-во подельщиков в группировке в ячейке DataGridView
        /// </summary>
        /// <param name="DGV">DataGridView в котором надо поменять кол-во подельщиков</param>
        public static void ChangeNumOfCriminalsInGroupInDataGV(DataGridView DGV)
        {
            foreach (DataGridViewRow row in DGV.Rows)
            {
                foreach (Group group in MyCollection.groups)
                {
                    if (row.Cells[0].Value.ToString() == group.Name)
                        row.Cells[1].Value = group.CountOfCriminals;
                }
            }
        }
    }
}