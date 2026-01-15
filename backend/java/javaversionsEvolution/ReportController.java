//Parte 2:Memory Management-Heap&Stack(25 puntos)
//4.(10 pts)Analiza este código de un endpoint Spring que causa OutOfMemoryError en producción:java @RestController @RequestMapping("/api/reports")
public class ReportController {

    @Autowired
    private UserRepository userRepository;

    @GetMapping("/all-users")
    public ResponseEntity<List<UserDTO>> getAllUsers() {
        List<User> users = userRepository.findAll(); // 10 millones de registros

        List<UserDTO> dtos = users.stream()
                .map(user -> new UserDTO(
                        user.getId(),
                        user.getName(),
                        user.getEmail(),
                        user.getCreatedAt()))
                .collect(Collectors.toList());

        return ResponseEntity.ok(dtos);
    }
}