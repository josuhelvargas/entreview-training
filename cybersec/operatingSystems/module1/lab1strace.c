execve("/usr/bin/ls", ["ls"], 0xfffffa264910 /* 77 vars */) = 0
brk(NULL)                               = 0xaaaad6385000
mmap(NULL, 8192, PROT_READ|PROT_WRITE, MAP_PRIVATE|MAP_ANONYMOUS, -1, 0) = 0xffffa91d0000
faccessat(AT_FDCWD, "/etc/ld.so.preload", R_OK) = -1 ENOENT (No such file or directory)
openat(AT_FDCWD, "/etc/ld.so.cache", O_RDONLY|O_CLOEXEC) = 3
fstat(3, {st_mode=S_IFREG|0644, st_size=116907, ...}) = 0
mmap(NULL, 116907, PROT_READ, MAP_PRIVATE, 3, 0) = 0xffffa917b000
close(3)                                = 0
openat(AT_FDCWD, "/lib/aarch64-linux-gnu/libselinux.so.1", O_RDONLY|O_CLOEXEC) = 3
read(3, "\177ELF\2\1\1\0\0\0\0\0\0\0\0\0\3\0\267\0\1\0\0\0\0\0\0\0\0\0\0\0"..., 832) = 832
fstat(3, {st_mode=S_IFREG|0644, st_size=198880, ...}) = 0
mmap(NULL, 337200, PROT_NONE, MAP_PRIVATE|MAP_ANONYMOUS|MAP_DENYWRITE, -1, 0) = 0xffffa9128000
mmap(0xffffa9130000, 271664, PROT_READ|PROT_EXEC, MAP_PRIVATE|MAP_FIXED|MAP_DENYWRITE, 3, 0) = 0xffffa9130000
munmap(0xffffa9128000, 32768)           = 0
munmap(0xffffa9173000, 30000)           = 0
mprotect(0xffffa915f000, 65536, PROT_NONE) = 0
mmap(0xffffa916f000, 8192, PROT_READ|PROT_WRITE, MAP_PRIVATE|MAP_FIXED|MAP_DENYWRITE, 3, 0x2f000) = 0xffffa916f000
mmap(0xffffa9171000, 5424, PROT_READ|PROT_WRITE, MAP_PRIVATE|MAP_FIXED|MAP_ANONYMOUS, -1, 0) = 0xffffa9171000
close(3)                                = 0
openat(AT_FDCWD, "/lib/aarch64-linux-gnu/libcap.so.2", O_RDONLY|O_CLOEXEC) = 3
read(3, "\177ELF\2\1\1\0\0\0\0\0\0\0\0\0\3\0\267\0\1\0\0\0\200{\0\0\0\0\0\0"..., 832) = 832
fstat(3, {st_mode=S_IFREG|0644, st_size=67792, ...}) = 0
mmap(NULL, 196704, PROT_NONE, MAP_PRIVATE|MAP_ANONYMOUS|MAP_DENYWRITE, -1, 0) = 0xffffa90ff000
mmap(0xffffa9100000, 131168, PROT_READ|PROT_EXEC, MAP_PRIVATE|MAP_FIXED|MAP_DENYWRITE, 3, 0) = 0xffffa9100000
munmap(0xffffa90ff000, 4096)            = 0
munmap(0xffffa9121000, 57440)           = 0
mprotect(0xffffa910a000, 86016, PROT_NONE) = 0
mmap(0xffffa911f000, 8192, PROT_READ|PROT_WRITE, MAP_PRIVATE|MAP_FIXED|MAP_DENYWRITE, 3, 0xf000) = 0xffffa911f000
close(3)                                = 0
openat(AT_FDCWD, "/lib/aarch64-linux-gnu/libc.so.6", O_RDONLY|O_CLOEXEC) = 3
read(3, "\177ELF\2\1\1\3\0\0\0\0\0\0\0\0\3\0\267\0\1\0\0\0\300$\2\0\0\0\0\0"..., 832) = 832
fstat(3, {st_mode=S_IFREG|0755, st_size=1782152, ...}) = 0
mmap(NULL, 1892400, PROT_NONE, MAP_PRIVATE|MAP_ANONYMOUS|MAP_DENYWRITE, -1, 0) = 0xffffa8f31000
mmap(0xffffa8f40000, 1826864, PROT_READ|PROT_EXEC, MAP_PRIVATE|MAP_FIXED|MAP_DENYWRITE, 3, 0) = 0xffffa8f40000
munmap(0xffffa8f31000, 61440)           = 0
munmap(0xffffa90ff000, 48)              = 0
mprotect(0xffffa90de000, 61440, PROT_NONE) = 0
mmap(0xffffa90ed000, 20480, PROT_READ|PROT_WRITE, MAP_PRIVATE|MAP_FIXED|MAP_DENYWRITE, 3, 0x1ad000) = 0xffffa90ed000
mmap(0xffffa90f2000, 49200, PROT_READ|PROT_WRITE, MAP_PRIVATE|MAP_FIXED|MAP_ANONYMOUS, -1, 0) = 0xffffa90f2000
close(3)                                = 0
openat(AT_FDCWD, "/lib/aarch64-linux-gnu/libpcre2-8.so.0", O_RDONLY|O_CLOEXEC) = 3
read(3, "\177ELF\2\1\1\0\0\0\0\0\0\0\0\0\3\0\267\0\1\0\0\0\0\0\0\0\0\0\0\0"..., 832) = 832
fstat(3, {st_mode=S_IFREG|0644, st_size=657960, ...}) = 0
mmap(NULL, 787088, PROT_NONE, MAP_PRIVATE|MAP_ANONYMOUS|MAP_DENYWRITE, -1, 0) = 0xffffa8e7f000
mmap(0xffffa8e80000, 721552, PROT_READ|PROT_EXEC, MAP_PRIVATE|MAP_FIXED|MAP_DENYWRITE, 3, 0) = 0xffffa8e80000
munmap(0xffffa8e7f000, 4096)            = 0
munmap(0xffffa8f31000, 58000)           = 0
mprotect(0xffffa8f17000, 98304, PROT_NONE) = 0
mmap(0xffffa8f2f000, 8192, PROT_READ|PROT_WRITE, MAP_PRIVATE|MAP_FIXED|MAP_DENYWRITE, 3, 0x9f000) = 0xffffa8f2f000
close(3)                                = 0
mmap(NULL, 8192, PROT_READ|PROT_WRITE, MAP_PRIVATE|MAP_ANONYMOUS, -1, 0) = 0xffffa91ce000
set_tid_address(0xffffa91ce510)         = 10715
set_robust_list(0xffffa91ce520, 24)     = 0
rseq(0xffffa91ced00, 0x20, 0, 0xd428bc00) = 0
mprotect(0xffffa90ed000, 12288, PROT_READ) = 0
mprotect(0xffffa8f2f000, 4096, PROT_READ) = 0
mprotect(0xffffa911f000, 4096, PROT_READ) = 0
mprotect(0xffffa916f000, 4096, PROT_READ) = 0
mprotect(0xaaaaab6ce000, 8192, PROT_READ) = 0
mprotect(0xffffa91d6000, 8192, PROT_READ) = 0
prlimit64(0, RLIMIT_STACK, NULL, {rlim_cur=8192*1024, rlim_max=RLIM64_INFINITY}) = 0
munmap(0xffffa917b000, 116907)          = 0
prctl(PR_CAPBSET_READ, CAP_MAC_OVERRIDE) = 1
prctl(PR_CAPBSET_READ, 0x30 /* CAP_??? */) = -1 EINVAL (Invalid argument)
prctl(PR_CAPBSET_READ, CAP_CHECKPOINT_RESTORE) = 1
prctl(PR_CAPBSET_READ, 0x2c /* CAP_??? */) = -1 EINVAL (Invalid argument)
prctl(PR_CAPBSET_READ, 0x2a /* CAP_??? */) = -1 EINVAL (Invalid argument)
prctl(PR_CAPBSET_READ, 0x29 /* CAP_??? */) = -1 EINVAL (Invalid argument)
statfs("/sys/fs/selinux", 0xffffeb9c43d0) = -1 ENOENT (No such file or directory)
statfs("/selinux", 0xffffeb9c43d0)      = -1 ENOENT (No such file or directory)
getrandom("\xa1\x73\xf0\xd5\x5b\xd7\xfb\x77", 8, GRND_NONBLOCK) = 8
brk(NULL)                               = 0xaaaad6385000
brk(0xaaaad63a6000)                     = 0xaaaad63a6000
openat(AT_FDCWD, "/proc/filesystems", O_RDONLY|O_CLOEXEC) = 3
fstat(3, {st_mode=S_IFREG|0444, st_size=0, ...}) = 0
read(3, "nodev\tsysfs\nnodev\ttmpfs\nnodev\tbd"..., 1024) = 401
read(3, "", 1024)                       = 0
close(3)                                = 0
faccessat(AT_FDCWD, "/etc/selinux/config", F_OK) = -1 ENOENT (No such file or directory)
openat(AT_FDCWD, "/usr/lib/locale/locale-archive", O_RDONLY|O_CLOEXEC) = 3
fstat(3, {st_mode=S_IFREG|0644, st_size=3063024, ...}) = 0
mmap(NULL, 3063024, PROT_READ, MAP_PRIVATE, 3, 0) = 0xffffa8a00000
close(3)                                = 0
ioctl(1, TCGETS, {c_iflag=BRKINT|ICRNL|IXON|IXANY|IMAXBEL|IUTF8, c_oflag=NL0|CR0|TAB0|BS0|VT0|FF0|OPOST|ONLCR, c_cflag=B38400|CS8|CREAD|HUPCL, c_lflag=ISIG|ICANON|ECHO|ECHOE|ECHOK|IEXTEN|ECHOCTL|ECHOKE, ...}) = 0
ioctl(1, TIOCGWINSZ, {ws_row=21, ws_col=175, ws_xpixel=0, ws_ypixel=0}) = 0
openat(AT_FDCWD, ".", O_RDONLY|O_NONBLOCK|O_CLOEXEC|O_DIRECTORY) = 3
fstat(3, {st_mode=S_IFDIR|0775, st_size=4096, ...}) = 0
getdents64(3, 0xaaaad638b6a0 /* 18 entries */, 32768) = 560
getdents64(3, 0xaaaad638b6a0 /* 0 entries */, 32768) = 0
close(3)                                = 0
fstat(1, {st_mode=S_IFCHR|0600, st_rdev=makedev(0x88, 0x1), ...}) = 0
write(1, "arquitecto  backend  cloud  cybe"..., 128arquitecto  backend  cloud  cybersec  devops  entreview-training.sln  frontend       LearnC  LearnCpp  mcp  mobile  settings.json  sql
) = 128
close(1)                                = 0
close(2)                                = 0
exit_group(0)                           = ?
+++ exited with 0 +++
                                      