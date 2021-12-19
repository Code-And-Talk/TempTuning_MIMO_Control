# TempTuning_MIMO_Control
 과학기술정보통신부 소프트웨어개발자(스마트팩토리) 양성과정 최종 팀프로젝트
 
 프로젝트명 : ILC를 이용한 Temperature Tuning 및 MIMO System 제어
 
 (ILC : Iterative Learning Control, MIMO : Multi Input Multi Output)
 
 개발팀 : [CAT (Code & Talk)](https://github.com/Code-And-Talk) : [박정란](https://github.com/uiop1370)(팀장), [이승수](https://github.com/seungsu-lee-dev), [이종원](https://github.com/LJ-W), [이홍기](https://github.com/Hong-code-maker)
 
 사용 언어 : Structured Text (ST), C#
 
 사용 소프트웨어 : TwinCAT3 (The Windows Control and Automation Technology) ([TC1200](https://www.beckhoff.com/ko-kr/products/automation/twincat/tc1xxx-twincat-3-base/tc1200.html))
 
 사용 IDE : Visual Studio
 
 사용 하드웨어 : Beckhoff PLC ([CX8110](https://www.beckhoff.com/ko-kr/products/ipc/embedded-pcs/cx8100-arm-cortex-a9/cx8110.html))
 
 사용 통신 기술 : EtherCAT (Ethernet for Control Automation Technology)
 
 개발기간 : 2021년 7월 17일 ~ 2021년 10월 23일

 주요 기능

 - PLC를 이용한 원하는 온도까지 올리고 균일하게 유지하는 등 온도를 자동으로 제어하는 시스템 구축 및 C# 윈폼을 이용한 UI 모니터링, 제어 구현

 - PID 제어 slave controller와 PI 제어 master controller 두 가지의 피드백 제어 루프를 결합한 CASCADE 제어를 이용하여 목표 온도까지 도달

 - 각 제어기의 온도 설정 값(Set Value)을 조절하여 글래스 온도를 균일하게 제어

 - ILC(반복 학습 제어)를 통하여 앞 공정의 데이터를 이용하여 Heat up시 글래스 편차를 점차 줄여나감

 - UI에서 실시간 온도를 차트로 확인, 에러 발생 시 UI에서 적색 등으로 확인, 이전 에러 Log 확인 가능

 - 비상 시 버튼을 눌러 제어를 중지하여 온도를 서서히 내릴 수 있음

GitHub URL : https://github.com/Code-And-Talk/TempTuning_MIMO_Control

![슬라이드1](https://user-images.githubusercontent.com/68325847/140008969-96e24c14-f16c-4dbf-a5f6-2ed9bc4dd50f.PNG)

![슬라이드2](https://user-images.githubusercontent.com/68325847/140008975-c7f5cee6-c9af-4d3b-8966-39eab8773ce8.PNG)

![슬라이드3](https://user-images.githubusercontent.com/68325847/140008978-2183d48a-2b11-4f00-a1f3-711e0440b380.PNG)

![슬라이드4](https://user-images.githubusercontent.com/68325847/140008983-2c49c132-6b7a-41cd-83e6-7f0306d4bbe8.gif)

![슬라이드5](https://user-images.githubusercontent.com/68325847/140008984-e1c2b8f3-0bff-45de-ad28-b44ce429a152.gif)

![슬라이드6](https://user-images.githubusercontent.com/68325847/140008988-1e633f82-69b0-4e9b-81a0-194616df4f6d.PNG)

![슬라이드7](https://user-images.githubusercontent.com/68325847/140008994-8ec28845-a1cb-4287-b974-3f3ffa74b034.gif)

![슬라이드8](https://user-images.githubusercontent.com/68325847/140009020-8ce9901d-ca03-455c-9606-fe04f6f2de76.gif)

![슬라이드9](https://user-images.githubusercontent.com/68325847/140009026-021ba9f3-c450-4d51-a85c-8ca611a19a97.gif)

![슬라이드10](https://user-images.githubusercontent.com/68325847/140009027-ae86e9da-3c86-433e-8b15-270f44f1b782.gif)

![슬라이드11](https://user-images.githubusercontent.com/68325847/140009030-e1058733-46ee-443d-9d1a-a3e621986ea4.gif)

[![슬라이드12](https://user-images.githubusercontent.com/68325847/140009034-a2c26514-34e5-4f8d-8676-7cc036674b2c.PNG)](https://youtu.be/vrP4LwtEAl0)

https://youtu.be/vrP4LwtEAl0

![슬라이드13](https://user-images.githubusercontent.com/68325847/140009036-73282141-b57e-4892-b11b-391336ee604a.PNG)

![슬라이드14](https://user-images.githubusercontent.com/68325847/140009040-98a4afb4-041e-41fc-94c3-7f6f0135b41d.PNG)
