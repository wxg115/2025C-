﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Week04Homework
{
    //열거형: 정수형
    enum YEAR
    {
        ONE,
        TWO,
        THREE,
        FOUR,
        END,
    }

    enum CLASS
    {
        A,
        B,
        C,
        END,
    }

    class Student {
        //static 필드, 메모리 한 곳에만 저장
        public static Dictionary<YEAR, string> YearName = new Dictionary<YEAR, string> {
            { YEAR.ONE, "1학년" },
            { YEAR.TWO, "2학년" },
            { YEAR.THREE, "3학년" },
            { YEAR.FOUR, "4학년(심화)" },
        };

        public string Number { get; private set; } //학번
        public string Name { get; private set; } //이름
        public DateTime BirthInfo { get; private set; } //생년월일
        public void SetBirthDay(int year, int month, int day)
        {
            BirthInfo = new DateTime(year, month, day);
        }
        public string DepartmentCode { get; set; } //학과코드
        public string AdvisorNumber { get; set; } //지도교수번호
        public YEAR Year { get; set; } //학년
        public CLASS Class { get; set; } //반
        public string RegStatus { get; set; } //재적상태
        public string Address { get; set; } //주소

        public Student(string number, string name)
        {
            Number = number;
            Name = name;
        }

        //자동 구현 프로퍼티
        //컴파일러가 __contact private 변수 생성 + get, set 구현
        public string Contact { get; set; }//연락처
        public override string ToString() {
            return $"[{this.Number}]{this.Name}";
        }
    }
}
