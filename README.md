# TempTuning_MIMO_Control
 과학기술정보통신부 소프트웨어개발자(스마트팩토리) 양성과정 최종 팀프로젝트
 
 프로젝트명 : ILC를 이용한 Temperature Tuning 및 MIMO System 제어
 
 (ILC : Iterative Learning Control, MIMO : Multi Input Multi Output)
 
 사용 언어 : Structured Text (ST), C#
 
 사용 소프트웨어 : TwinCAT3 (The Windows Control and Automation Technology)
 
 사용 IDE : Visual Studio
 
 사용 하드웨어 : Beckhoff PLC
 
 사용 통신 기술 : EtherCAT (Ethernet for Control Automation Technology)
 
 개발기간 : 2021년 7월 17일 ~ 2021년 10월 23일
 
 개발팀 : CAT (Code & Talk)

 주요 기능

 - PLC를 이용한 원하는 온도까지 올리고 균일하게 유지하는 등 온도를 자동으로 제어하는 시스템 구축 및 C# 윈폼을 이용한 UI 모니터링, 제어 구현

 - PID 제어 slave controller와 PI 제어 master controller 두 가지의 피드백 제어 루프를 결합한 CASCADE 제어를 이용하여 목표 온도까지 도달

 - 각 제어기의 온도 설정 값(Set Value)을 조절하여 글래스 온도를 균일하게 제어

 - ILC(반복 학습 제어)를 통하여 앞 공정의 데이터를 이용하여 Heat up시 글래스 편차를 점차 줄여나감

 - UI에서 실시간 온도를 차트로 확인, 에러 발생 시 UI에서 적색 등으로 확인, 이전 에러 Log 확인 가능

 - 비상 시 버튼을 눌러 제어를 중지하여 온도를 서서히 내릴 수 있음
