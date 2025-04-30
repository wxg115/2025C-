﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Week04Homework
{
    //Object 클래스를 상속한 Department
    class Department : object
    {
        //인스턴스 필드
        private string _code;
        private string _name;

        //속성, property
        //getter + setter 한꺼번에 구현
        //정의는 method와 유사
        //사용은 변수와 유사
        public string Code
        {
            get { return _code; }
            private set
            {
                if (false == string.IsNullOrEmpty(value))
                {
                    _code = value;
                }
                else
                {
                    _code = "ERR" + value;
                }
            }
        }

        public string Name
        {
            get { return _name; }
            private set { if (false == string.IsNullOrEmpty(value))
                {
                    _name = value;
                }
                else
                {
                    _name = "[ERR]" + value;
                }
            }
        }

        //getter
        public string GetCode()
        {
            return _code;
        }
        //setter
        private void SetCode(string value)
        {
            if (false == string.IsNullOrEmpty(value))
            {
                _code = value;
            }
        }

        //python의 __str__()와 동일
        //인스턴스 메소드
        public override string ToString()
        {
            return $"[{Code}] {Name}";
        }

        //생성자, constructor
        //반환 타입 X
        //클래스 이름 동일 O

        //메소드 오버로딩
        //동일한 메소드 이름을 만들 수 있음
        //방법: 매개변수의 갯수나 형을 다르게 하면 됨

        //기본 생성자
        //컴파일러가 만들어줌
        //조건, 하나라도 사용자가 만든 생성자가 있다면 만들지 않음
        public Department()
        {
        
        }

        public Department(string code, string name)
        {
            Code = code;
            Name = name;
        }
    }
}
