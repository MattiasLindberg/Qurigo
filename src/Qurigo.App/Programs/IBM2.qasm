﻿OPENQASM 2.0;
include "qelib1.inc";
gate rzx(param0) q0,q1 { h q1; cx q0,q1; rz(param0) q1; cx q0,q1; h q1; }
gate ecr q0,q1 { rzx(pi/4) q0,q1; x q0; rzx(-pi/4) q0,q1; }
qreg q[3];
creg meas[3];
rz(-pi/2) q[0];
sx q[0];
rz(1.9659526895380157) q[0];
rz(pi/2) q[1];
sx q[1];
rz(pi/4) q[2];
sx q[2];
rz(pi/2) q[2];
ecr q[0], q[2];
rz(0.3951563627431218) q[0];
x q[0];
rz(-2.5261129449194066) q[2];
sx q[2];
rz(-pi/3) q[2];
sx q[2];
rz(-2.186276035465286) q[2];
ecr q[1], q[2];
rz(-1.3045555541480935) q[1];
sx q[1];
rz(-pi/2) q[1];
rz(-pi/4) q[2];
sx q[2];
rz(-2.301250813921845) q[2];
sx q[2];
rz(pi/2) q[2];
ecr q[0], q[2];
rz(pi/4) q[0];
rz(0.6402106147957474) q[2];
sx q[2];
rz(-1.0794793747731042) q[2];
sx q[2];
rz(-0.5646035839116266) q[2];
ecr q[1], q[2];
sx q[1];
rz(pi/2) q[1];
sx q[2];
rz(-pi/2) q[2];
ecr q[1], q[2];
rz(0.022832085484227882) q[1];
sx q[1];
rz(-pi) q[1];
rz(1.0369004485150057) q[2];
sx q[2];
rz(-3*pi/4) q[2];
sx q[2];
rz(-pi/2) q[2];
ecr q[0], q[2];
rz(-0.2849241266220641) q[2];
sx q[2];
rz(-2.59356424596948) q[2];
sx q[2];
rz(2.8566685269677325) q[2];
ecr q[0], q[2];
x q[0];
rz(-pi/2) q[0];
rz(-3.131170591800279) q[2];
sx q[2];
rz(-1.5812178226116114) q[2];
sx q[2];
rz(1.5479099335909208) q[2];
ecr q[1], q[2];
rz(-pi/2) q[1];
sx q[1];
rz(pi/2) q[1];
x q[2];
rz(pi/2) q[2];
ecr q[1], q[2];
sx q[1];
rz(pi/2) q[1];
sx q[2];
rz(-pi/2) q[2];
ecr q[1], q[2];
rz(2.3790265756765736) q[1];
sx q[1];
rz(pi/2) q[1];
rz(pi/2) q[2];
sx q[2];
rz(2.3790265756765736) q[2];
sx q[2];
ecr q[0], q[2];
rz(pi/2) q[0];
rz(3*pi/4) q[2];
sx q[2];
rz(-pi) q[2];
ecr q[1], q[2];
rz(0.3826165127652743) q[1];
sx q[1];
rz(pi/2) q[1];
rz(-pi/4) q[2];
sx q[2];
rz(-2.301250813921845) q[2];
sx q[2];
rz(pi/2) q[2];
ecr q[0], q[2];
rz(pi/4) q[0];
sx q[0];
rz(-pi/2) q[0];
rz(0.6402106147957474) q[2];
sx q[2];
rz(-1.0794793747731042) q[2];
sx q[2];
rz(2.5769890696781657) q[2];
ecr q[1], q[2];
sx q[1];
rz(pi/2) q[1];
sx q[2];
rz(-pi/2) q[2];
ecr q[1], q[2];
rz(-pi/4) q[1];
sx q[1];
rz(2.333362404708116) q[1];
sx q[1];
rz(-2.901782311194099) q[2];
sx q[2];
rz(pi/2) q[2];
ecr q[0], q[2];
sx q[0];
rz(-3*pi/4) q[0];
sx q[0];
rz(-pi/2) q[0];
rz(-pi) q[2];
sx q[2];
rz(-pi) q[2];
ecr q[0], q[2];
rz(pi/2) q[0];
sx q[0];
rz(-pi/2) q[0];
rz(-pi/2) q[2];
sx q[2];
rz(-1.7337676350313753) q[2];
sx q[2];
rz(0.7625660779132204) q[2];
ecr q[1], q[2];
rz(-pi/2) q[1];
sx q[1];
rz(pi/2) q[1];
x q[2];
rz(pi/2) q[2];
ecr q[1], q[2];
sx q[1];
rz(pi/2) q[1];
sx q[2];
rz(-pi/2) q[2];
ecr q[1], q[2];
rz(2.3790265756765736) q[1];
sx q[1];
rz(pi/2) q[1];
rz(pi/2) q[2];
sx q[2];
rz(2.3790265756765736) q[2];
sx q[2];
ecr q[0], q[2];
x q[0];
rz(-pi/2) q[0];
rz(3*pi/4) q[2];
sx q[2];
rz(-pi) q[2];
ecr q[1], q[2];
x q[1];
rz(-pi/2) q[1];
rz(-3*pi/4) q[2];
sx q[2];
rz(-pi) q[2];
ecr q[0], q[2];
rz(3.1187605681055643) q[0];
sx q[0];
rz(3*pi/4) q[2];
sx q[2];
rz(-pi) q[2];
ecr q[1], q[2];
x q[1];
rz(-pi/2) q[1];
x q[2];
rz(-0.02283208548422877) q[2];
ecr q[0], q[2];
rz(-pi/2) q[0];
sx q[0];
rz(pi/2) q[0];
x q[2];
rz(pi/2) q[2];
ecr q[0], q[2];
sx q[0];
rz(pi/2) q[0];
sx q[2];
rz(-pi/2) q[2];
ecr q[0], q[2];
rz(2.3790265756765736) q[0];
sx q[0];
rz(pi/2) q[0];
rz(pi/2) q[2];
sx q[2];
rz(2.3790265756765736) q[2];
sx q[2];
ecr q[1], q[2];
x q[1];
rz(-pi/4) q[1];
rz(3*pi/4) q[2];
sx q[2];
rz(-pi) q[2];
ecr q[1], q[2];
rz(pi/2) q[1];
sx q[1];
rz(pi/2) q[1];
rz(pi/2) q[2];
sx q[2];
rz(-pi/2) q[2];
barrier q[1], q[2], q[0];
measure q[1] -> meas[0];
measure q[2] -> meas[1];
measure q[0] -> meas[2];