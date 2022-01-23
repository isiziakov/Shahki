using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Shahki
{
    [Serializable]
    public class II
    {
        public int[,] best_turn = new int[2,13];
        public II()
        {

        }

        public void Bot_eat(Field field, int step, int position, int[,] turn, int count, int score, ref int best_score/*, int color*/)
        {
            int[] second_position = new int[2], food = new int[2], first_position = new int[2];
            int current_score = score; ; // запись счета, полученного ранее
            Field copy = new Field();
            if (field.cells[position % 100 / 10, position % 10].item.Check_eating()) // если шашка из клетки position может съесть шашку
            {
                for (int i = 0; field.cells[position % 100 / 10, position % 10].item.turn[i] != 0; i++) // и проверка для всех этих позиций
                {
                    score = current_score; // откат счета до текущего состояния
                    first_position[0] = position % 100 / 10;
                    first_position[1] = position % 10;
                    second_position[0] = field.cells[position % 100 / 10, position % 10].item.turn[i] % 100 / 10;
                    second_position[1] = field.cells[position % 100 / 10, position % 10].item.turn[i] % 10;
                    food[0] = field.cells[position % 100 / 10, position % 10].item.turn[i] / 1000;
                    food[1] = field.cells[position % 100 / 10, position % 10].item.turn[i] % 1000 / 100;
                    if (field.cells[food[0], food[1]].item is King) // если съедена дамка
                    {
                        score += 9;
                        if (step % 2 == 1) // здесь и далее применение значения из настроек для хода компьютера
                        {
                            score += Program.setting.e_king * 3;
                        }
                    }
                    else
                    {
                        score += 3; // съедена простая шашка
                        if (step % 2 == 1)
                        {
                            score += Program.setting.attack;
                        }
                        else
                        {
                            score -= Program.setting.defense;
                        }
                    }
                    if (field.cells[first_position[0], first_position[1]].item is King) // проверка позиций для дамки
                    {
                        if (first_position[0] == first_position[1]) // дамка на главной диагонали
                        {
                            if (second_position[0] != second_position[1]) // дамка покинула главную диагональ
                            {
                                score -= 2;
                                if (step % 2 == 1)
                                {
                                    score -= Program.setting.pos;
                                }
                            }
                        }
                        else
                        {
                            if (second_position[0] == second_position[1]) // дамка встала на главную диагональ
                            {
                                score += 2;
                                if (step % 2 == 1)
                                {
                                    score += Program.setting.pos;
                                }
                            }
                        }
                    }
                    else // проверка позиций для не дамки
                    {
                        if (field.player == 1 && first_position[0] == 7) // белая шашка покинула строку 8
                        {
                            score -= 1;
                            if (step % 2 == 1)
                            {
                                score -= Program.setting.pos;
                            }
                        }
                        if (field.player == 2 && first_position[0] == 0) // черная шашка покинула строку 1
                        {
                            score -= 1;
                            if (step % 2 == 1)
                            {
                                score -= Program.setting.pos;
                            }
                        }
                        if (field.player == 1 && second_position[0] == 7) // белая шашка вернулась на строку 8
                        {
                            score += 1;
                            if (step % 2 == 1)
                            {
                                score += Program.setting.pos;
                            }
                        }
                        if (field.player == 2 && second_position[0] == 0) // черная шашка вернулась на строку 1
                        {
                            score += 1;
                            if (step % 2 == 1)
                            {
                                score += Program.setting.pos;
                            }
                        }
                        if (field.player == 1 && second_position[0] == 1 && second_position[1] == 0) // белая шашка заняла позицию h2
                        {
                            score += 2;
                            if (step % 2 == 1)
                            {
                                score += Program.setting.pos;
                            }
                        }
                        if (field.player == 2 && second_position[0] == 6 && second_position[1] == 7) // черная шашка заняла позицию a7
                        {
                            score += 2;
                            if (step % 2 == 1)
                            {
                                score += Program.setting.pos;
                            }
                        }
                    }
                    if (step == 1) // если текущая глубина проверки = 1
                    {
                        turn[0, count] = position; // записываем ход
                        turn[1, count] = second_position[0] * 10 + second_position[1];
                        count++; // увеличиваем счетчик записанных ходов (съедений подряд)
                    }
                    copy = new Field(field);
                    if (copy.cells[first_position[0], first_position[1]].item.Bot_make_eat(second_position[0], second_position[1], food[0], food[1])) // съедение и проверка стала ли шашка дамкой
                    {
                        score += 9;
                        if (step % 2 == 1)
                        {
                            score += Program.setting.m_king;
                        }
                        else
                        {
                            score += Program.setting.c_king;
                        }
                    }
                    Bot_eat(copy, step, second_position[0] * 10 + second_position[1], turn, count, score, ref best_score); // проверка возможно ли продолжение хода
                    if (step == 1) // откат на прошлый шаг
                    {
                        count--; // уменьшение счетчика
                        turn[0, count] = 0; // удаление хода
                        turn[1, count] = 0;
                    }
                }
            }
            else
            {
                copy = new Field(field);
                copy.Check_eating();
                if (!copy.eating) // все шашки противника заблокированы или кончились
                {
                    copy.Check_moving();
                    if (!copy.have_turn)
                    {
                        score += 30;
                        best_score = score;
                        if (step == 1) // если шаг 1 - запись ходов в лучшие
                        {
                            best_turn = turn; // запись текущего хода в лучший
                        }
                    }
                }
                if (step < Program.setting.turns * 2) // пока текущая глубина проверки не больше максимальной
                {
                    copy.player += 1;
                    if (copy.player == 3)
                    {
                        copy.player -= 2;
                    }
                    score -= Bot_turn_analisis(copy, step + 1); // нахождение лучшего хода противника
                }
                if (score > best_score) // если текущий счет больше лучшего
                {
                    if (step == 1) // если шаг 1 - запись ходов в лучшие
                    { 
                        best_turn = (int[,])turn.Clone(); // запись текущего хода в лучший
                    }
                    best_score = score; // перезапись лучшего счета
                }
                Random rnd = new Random();
                if (score == best_score && rnd.Next() % 2 == 1) // если лучший ход совпадает с текущим, с вероятностью 0,5 перепишем лучший ход
                {
                    if (step == 1) // если шаг 1 - запись ходов в лучшие
                    {
                        best_turn = (int[,])turn.Clone(); // запись текущего хода в лучший
                    }
                    best_score = score; // перезапись лучшего счета
                }
            }
        }

        public int Bot_turn_analisis(Field field, int step)
        {
            int[,] turn = new int[2,13]; // текущий ход
            int score = 0, best_score = -1000000; // лучший и текщий счет, изначально отрицательны
            field.Check_eating();
            if (field.eating) // если можно съесть шашку
            {
                for (int i = 0; i < 8; i++) // перебор всех шашек
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (field.cells[i, j].item != null && field.cells[i, j].item.turn[0] != 0) // если цвет шашки совпадает с текущим цветом
                        {
                            for (int w = 0; field.cells[i, j].item.turn[w] != 0; w++) // совершение всех возможных ходов
                            {
                                Field copy = new Field(field);
                                int position = i * 10 + j;
                                Bot_eat(copy, step, position, turn, 0, score, ref best_score);
                            }
                        }
                    }
                }
            }
            else
            {
                field.Check_moving();
                for (int i = 0; i < 8; i++) // перебор всех шашек
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (field.cells[i, j].item != null && field.cells[i, j].item.turn[0] != 0) // если цвет шашки совпадает с текущим цветом
                        {
                            for (int w = 0; field.cells[i, j].item.turn[w] != 0; w++) // совершение всех возможных ходов
                            {
                                Field copy = new Field(field);
                                score = 0; // текущий счет 0
                                if (copy.cells[i, j].item is King) // увеличение счета от позиций см. выше
                                {
                                    if (i == j)
                                    {
                                        if (copy.cells[i, j].item.turn[w] % 100 / 10 != copy.cells[i, j].item.turn[w] % 10)
                                        {
                                            score -= 2;
                                            if (step % 2 == 1)
                                            {
                                                score -= Program.setting.pos;
                                            }
                                        }
                                    }
                                    if (i != j)
                                    {
                                        if (copy.cells[i, j].item.turn[w] % 100 / 10 == copy.cells[i, j].item.turn[w] % 10)
                                        {
                                            score += 2;
                                            if (step % 2 == 1)
                                            {
                                                score += Program.setting.pos;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (copy.player == 1 && i == 7)
                                    {
                                        score -= 1;
                                        if (step % 2 == 1)
                                        {
                                            score -= Program.setting.pos;
                                        }
                                    }
                                    if (copy.player == 2 && i == 0)
                                    {
                                        score -= 1;
                                        if (step % 2 == 1)
                                        {
                                            score -= Program.setting.pos;
                                        }
                                    }
                                    if (copy.player == 1 && copy.cells[i, j].item.turn[w] % 100 / 10 == 1 && copy.cells[i, j].item.turn[w] % 10 == 0)
                                    {
                                        score += 2;
                                        if (step % 2 == 1)
                                        {
                                            score += Program.setting.pos;
                                        }
                                    }
                                    if (copy.player == 2 && copy.cells[i, j].item.turn[w] % 100 / 10 == 6 && copy.cells[i, j].item.turn[w] % 10 == 7)
                                    {
                                        score += 2;
                                        if (step % 2 == 1)
                                        {
                                            score += Program.setting.pos;
                                        }
                                    }
                                }
                                if (step == 1) // запись хода
                                {
                                    turn[0, 0] = i * 10 + j;
                                    turn[1, 0] = copy.cells[i, j].item.turn[w];
                                }
                                if (copy.cells[i, j].item.Bot_make_turn(copy.cells[i, j].item.turn[w] % 100 / 10, copy.cells[i, j].item.turn[w] % 10)) // если шашка стала дамкой
                                {
                                    score += 9;
                                    if (step % 2 == 1)
                                    {
                                        score += Program.setting.m_king;
                                    }
                                    else
                                    {
                                        score += Program.setting.c_king;
                                    }
                                }
                                copy.Check_eating();
                                if (!copy.eating) // все шашки противника заблокированы или кончились
                                {
                                    copy.Check_moving();
                                    if (!copy.have_turn)
                                    {
                                        score += 30;
                                        if (step == 1)
                                        {
                                            best_score = score;
                                            best_turn[0, 0] = turn[0, 0];
                                            best_turn[1, 0] = turn[1, 0];
                                        }
                                        return score;
                                    }
                                }
                                if (step < Program.setting.turns * 2) // если шаг меньше глубины * 2
                                {
                                    copy.player += 1;
                                    if (copy.player == 3)
                                    {
                                        copy.player -= 2;
                                    }
                                    score -= Bot_turn_analisis(copy, step + 1); // рассчет лучшего хода для второго игрока
                                }
                                if (score > best_score) // определение лучшего хода / счета (подробно см. выше)
                                {
                                    if (step == 1)
                                    {
                                        best_score = score;
                                        best_turn[0, 0] = turn[0, 0];
                                        best_turn[1, 0] = turn[1, 0] = field.cells[i, j].item.turn[w];
                                    }
                                    else
                                    {
                                        best_score = score;
                                    }
                                }
                                Random rnd = new Random();
                                if (score == best_score && rnd.Next() % 2 == 1)
                                {
                                    if (step == 1)
                                    {
                                        best_score = score;
                                        best_turn[0, 0] = turn[0, 0];
                                        best_turn[1, 0] = turn[1, 0] = field.cells[i, j].item.turn[w];
                                    }
                                    else
                                    {
                                        best_score = score;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (best_score == -1000000) // если счет не менялся
            {
                best_score = 0;
            }
            return best_score /** ((settings.turns + 1) / 2)*/; // возвращает лучший найденный счет
        }
    }
}
