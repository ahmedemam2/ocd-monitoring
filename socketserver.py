import socket
socket = socket.socket()  # step 1
hostname = '127.0.0.1'
port = 65434
socket.bind((hostname, port))  # step 2

serverAddress = ((hostname, port))
list = [50, 100]
socket.listen(5)

conn, addr = socket.accept()
print("device connected")

for i in range(len(list)):
    msg = list

    Data1 = str.encode(str(msg))
    conn.send(Data1)