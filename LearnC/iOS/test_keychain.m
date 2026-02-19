#import <Foundation/Foundation.h>
#import <Security/Security.h>

int main(int argc, const char * argv[]) {
    @autoreleasepool {
        NSDictionary *addQuery = @{
            (__bridge id)kSecClass:      (__bridge id)kSecClassGenericPassword,
            (__bridge id)kSecAttrAccount:(__bridge id)CFSTR("demo-account"),
            (__bridge id)kSecValueData:  [@"secret" dataUsingEncoding:NSUTF8StringEncoding]
        };
        // Limpia item previo
        SecItemDelete((__bridge CFDictionaryRef)addQuery);

        OSStatus st = SecItemAdd((__bridge CFDictionaryRef)addQuery, NULL);
        NSLog(@"SecItemAdd status: %d", (int)st);

        NSDictionary *copyQuery = @{
            (__bridge id)kSecClass:       (__bridge id)kSecClassGenericPassword,
            (__bridge id)kSecAttrAccount: (__bridge id)CFSTR("demo-account"),
            (__bridge id)kSecReturnData:  @YES
        };
        CFTypeRef result = NULL;
        st = SecItemCopyMatching((__bridge CFDictionaryRef)copyQuery, &result);
        NSLog(@"SecItemCopyMatching status: %d", (int)st);
        if (st == errSecSuccess) {
            NSData *data = (__bridge_transfer NSData *)result;
            NSLog(@"Found data: %@", [[NSString alloc] initWithData:data encoding:NSUTF8StringEncoding]);
        }
    }
    return 0;
}
