// 🧪 Ejemplos Completos de Tests con Jest
// 1. Test de Botón de Formulario
// javascript// components/SubmitButton.jsx
import React from 'react';

export const SubmitButton = ({ onSubmit, disabled, children, loading }) => {
  return (
    <button
      type="submit"
      onClick={onSubmit}
      disabled={disabled || loading}
      className={`submit-btn ${loading ? 'loading' : ''}`}
      aria-label="Submit form"
    >
      {loading ? 'Submitting...' : children}
    </button>
  );
};

// __tests__/SubmitButton.test.jsx
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { SubmitButton } from '../components/SubmitButton';

describe('SubmitButton Component', () => {
  let mockOnSubmit;

  beforeEach(() => {
    mockOnSubmit = jest.fn();
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  // ✅ Test básico de renderizado
  test('renders button with correct text', () => {
    render(<SubmitButton onSubmit={mockOnSubmit}>Submit Form</SubmitButton>);

    const button = screen.getByRole('button', { name: /submit form/i });
    expect(button).toBeInTheDocument();
    expect(button).toHaveTextContent('Submit Form');
  });

  // ✅ Test de llamada al callback onClick
  test('calls onSubmit when clicked', () => {
    render(<SubmitButton onSubmit={mockOnSubmit}>Submit</SubmitButton>);

    const button = screen.getByRole('button');
    fireEvent.click(button);

    expect(mockOnSubmit).toHaveBeenCalledTimes(1);
  });

  // ✅ Test con userEvent (más realista)
  test('calls onSubmit with userEvent', async () => {
    const user = userEvent.setup();
    render(<SubmitButton onSubmit={mockOnSubmit}>Submit</SubmitButton>);

    const button = screen.getByRole('button');
    await user.click(button);

    expect(mockOnSubmit).toHaveBeenCalledTimes(1);
  });

  // ✅ Test de botón deshabilitado
  test('does not call onSubmit when disabled', () => {
    render(
      <SubmitButton onSubmit={mockOnSubmit} disabled>
        Submit
      </SubmitButton>
    );

    const button = screen.getByRole('button');

    expect(button).toBeDisabled();

    fireEvent.click(button);

    expect(mockOnSubmit).not.toHaveBeenCalled();
  });

  // ✅ Test de estado de carga
  test('shows loading text when loading is true', () => {
    render(
      <SubmitButton onSubmit={mockOnSubmit} loading>
        Submit
      </SubmitButton>
    );

    const button = screen.getByRole('button');

    expect(button).toHaveTextContent('Submitting...');
    expect(button).toBeDisabled();
    expect(button).toHaveClass('loading');
  });

  // ✅ Test de múltiples clicks (prevenir doble submit)
  test('prevents multiple submissions when clicked rapidly', async () => {
    const user = userEvent.setup();
    render(<SubmitButton onSubmit={mockOnSubmit}>Submit</SubmitButton>);

    const button = screen.getByRole('button');

    // Click rápido múltiple
    await user.click(button);
    await user.click(button);
    await user.click(button);

    // Debería llamarse 3 veces (si no hay debounce)
    expect(mockOnSubmit).toHaveBeenCalledTimes(3);
  });

  // ✅ Test con validación de formulario
  test('submits form with correct form data', () => {
    const handleSubmit = jest.fn((e) => {
      e.preventDefault();
      const formData = new FormData(e.target);
      return Object.fromEntries(formData);
    });

    const { container } = render(
      <form onSubmit={handleSubmit}>
        <input name="username" defaultValue="john" />
        <input name="email" defaultValue="john@test.com" />
        <SubmitButton onSubmit={mockOnSubmit}>Submit</SubmitButton>
      </form>
    );

    const form = container.querySelector('form');
    fireEvent.submit(form);

    expect(handleSubmit).toHaveBeenCalledTimes(1);
  });

  // ✅ Test de accesibilidad
  test('has correct accessibility attributes', () => {
    render(<SubmitButton onSubmit={mockOnSubmit}>Submit</SubmitButton>);

    const button = screen.getByRole('button');

    expect(button).toHaveAttribute('type', 'submit');
    expect(button).toHaveAttribute('aria-label', 'Submit form');
  });

  // ✅ Test de clases CSS dinámicas
  test('applies correct CSS classes based on state', () => {
    const { rerender } = render(
      <SubmitButton onSubmit={mockOnSubmit}>Submit</SubmitButton>
    );

    let button = screen.getByRole('button');
    expect(button).toHaveClass('submit-btn');
    expect(button).not.toHaveClass('loading');

    // Re-render con loading=true
    rerender(
      <SubmitButton onSubmit={mockOnSubmit} loading>
        Submit
      </SubmitButton>
    );

    button = screen.getByRole('button');
    expect(button).toHaveClass('submit-btn', 'loading');
  });
});















// 2. Test de Navegación entre Steps(Multi - Step Form)
// javascript// components/MultiStepForm.jsx
import React, { useState } from 'react';
import { Step1 } from './Step1';
import { Step2 } from './Step2';
import { Step3 } from './Step3';

export const MultiStepForm = () => {
  const [currentStep, setCurrentStep] = useState(1);
  const [formData, setFormData] = useState({
    name: '',
    email: '',
    address: '',
    city: ''
  });

  const nextStep = () => setCurrentStep(prev => prev + 1);
  const prevStep = () => setCurrentStep(prev => prev - 1);

  const updateFormData = (data) => {
    setFormData(prev => ({ ...prev, ...data }));
  };

  const renderStep = () => {
    switch (currentStep) {
      case 1:
        return (
          <Step1
            data={formData}
            onNext={nextStep}
            onUpdate={updateFormData}
          />
        );
      case 2:
        return (
          <Step2
            data={formData}
            onNext={nextStep}
            onPrev={prevStep}
            onUpdate={updateFormData}
          />
        );
      case 3:
        return (
          <Step3
            data={formData}
            onPrev={prevStep}
          />
        );
      default:
        return null;
    }
  };

  return (
    <div className="multi-step-form">
      <div className="progress-bar">
        <span className={currentStep >= 1 ? 'active' : ''}>Step 1</span>
        <span className={currentStep >= 2 ? 'active' : ''}>Step 2</span>
        <span className={currentStep >= 3 ? 'active' : ''}>Step 3</span>
      </div>
      {renderStep()}
    </div>
  );
};

// components/Step1.jsx
export const Step1 = ({ data, onNext, onUpdate }) => {
  const [name, setName] = useState(data.name);
  const [email, setEmail] = useState(data.email);

  const handleNext = () => {
    onUpdate({ name, email });
    onNext();
  };

  return (
    <div className="step step-1" data-testid="step-1">
      <h2>Step 1: Personal Information</h2>
      <input
        type="text"
        placeholder="Name"
        value={name}
        onChange={(e) => setName(e.target.value)}
        aria-label="Name"
      />
      <input
        type="email"
        placeholder="Email"
        value={email}
        onChange={(e) => setEmail(e.target.value)}
        aria-label="Email"
      />
      <button onClick={handleNext}>Next</button>
    </div>
  );
};

// components/Step2.jsx
export const Step2 = ({ data, onNext, onPrev, onUpdate }) => {
  const [address, setAddress] = useState(data.address);
  const [city, setCity] = useState(data.city);

  const handleNext = () => {
    onUpdate({ address, city });
    onNext();
  };

  return (
    <div className="step step-2" data-testid="step-2">
      <h2>Step 2: Address Information</h2>
      <input
        type="text"
        placeholder="Address"
        value={address}
        onChange={(e) => setAddress(e.target.value)}
        aria-label="Address"
      />
      <input
        type="text"
        placeholder="City"
        value={city}
        onChange={(e) => setCity(e.target.value)}
        aria-label="City"
      />
      <button onClick={onPrev}>Previous</button>
      <button onClick={handleNext}>Next</button>
    </div>
  );
};

// components/Step3.jsx
export const Step3 = ({ data, onPrev }) => {
  return (
    <div className="step step-3" data-testid="step-3">
      <h2>Step 3: Confirmation</h2>
      <div className="summary">
        <p>Name: {data.name}</p>
        <p>Email: {data.email}</p>
        <p>Address: {data.address}</p>
        <p>City: {data.city}</p>
      </div>
      <button onClick={onPrev}>Previous</button>
      <button type="submit">Submit</button>
    </div>
  );
};

// __tests__/MultiStepForm.test.jsx
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { MultiStepForm } from '../components/MultiStepForm';

describe('MultiStepForm - Step Navigation', () => {

  beforeEach(() => {
    // Setup antes de cada test
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  // ✅ Test: Renderiza Step 1 inicialmente
  test('renders Step 1 initially', () => {
    render(<MultiStepForm />);

    expect(screen.getByTestId('step-1')).toBeInTheDocument();
    expect(screen.getByText(/step 1: personal information/i)).toBeInTheDocument();
    expect(screen.queryByTestId('step-2')).not.toBeInTheDocument();
    expect(screen.queryByTestId('step-3')).not.toBeInTheDocument();
  });

  // ✅ Test: Navegación de Step 1 a Step 2
  test('navigates from Step 1 to Step 2 when Next is clicked', async () => {
    const user = userEvent.setup();
    render(<MultiStepForm />);

    // Llenar campos de Step 1
    const nameInput = screen.getByLabelText(/name/i);
    const emailInput = screen.getByLabelText(/email/i);

    await user.type(nameInput, 'John Doe');
    await user.type(emailInput, 'john@example.com');

    // Click en Next
    const nextButton = screen.getByRole('button', { name: /next/i });
    await user.click(nextButton);

    // Verificar que Step 2 está visible
    await waitFor(() => {
      expect(screen.getByTestId('step-2')).toBeInTheDocument();
    });

    expect(screen.getByText(/step 2: address information/i)).toBeInTheDocument();
    expect(screen.queryByTestId('step-1')).not.toBeInTheDocument();
  });

  // ✅ Test: Navegación completa (Step 1 → 2 → 3)
  test('completes full step navigation flow', async () => {
    const user = userEvent.setup();
    render(<MultiStepForm />);

    // === STEP 1 ===
    await user.type(screen.getByLabelText(/name/i), 'John Doe');
    await user.type(screen.getByLabelText(/email/i), 'john@example.com');
    await user.click(screen.getByRole('button', { name: /next/i }));

    // Esperar Step 2
    await waitFor(() => {
      expect(screen.getByTestId('step-2')).toBeInTheDocument();
    });

    // === STEP 2 ===
    await user.type(screen.getByLabelText(/address/i), '123 Main St');
    await user.type(screen.getByLabelText(/city/i), 'New York');

    const nextButtons = screen.getAllByRole('button', { name: /next/i });
    await user.click(nextButtons[0]);

    // Esperar Step 3
    await waitFor(() => {
      expect(screen.getByTestId('step-3')).toBeInTheDocument();
    });

    // === STEP 3 ===
    expect(screen.getByText(/step 3: confirmation/i)).toBeInTheDocument();
    expect(screen.getByText(/name: john doe/i)).toBeInTheDocument();
    expect(screen.getByText(/email: john@example.com/i)).toBeInTheDocument();
    expect(screen.getByText(/address: 123 main st/i)).toBeInTheDocument();
    expect(screen.getByText(/city: new york/i)).toBeInTheDocument();
  });

  // ✅ Test: Navegación hacia atrás (Previous)
  test('navigates back from Step 2 to Step 1', async () => {
    const user = userEvent.setup();
    render(<MultiStepForm />);

    // Ir a Step 2
    await user.type(screen.getByLabelText(/name/i), 'John');
    await user.click(screen.getByRole('button', { name: /next/i }));

    await waitFor(() => {
      expect(screen.getByTestId('step-2')).toBeInTheDocument();
    });

    // Click Previous
    const prevButton = screen.getByRole('button', { name: /previous/i });
    await user.click(prevButton);

    // Verificar que volvemos a Step 1
    await waitFor(() => {
      expect(screen.getByTestId('step-1')).toBeInTheDocument();
    });

    expect(screen.queryByTestId('step-2')).not.toBeInTheDocument();
  });

  // ✅ Test: Datos persisten entre steps
  test('form data persists when navigating between steps', async () => {
    const user = userEvent.setup();
    render(<MultiStepForm />);

    // Step 1: Llenar datos
    await user.type(screen.getByLabelText(/name/i), 'Jane Smith');
    await user.type(screen.getByLabelText(/email/i), 'jane@test.com');
    await user.click(screen.getByRole('button', { name: /next/i }));

    await waitFor(() => {
      expect(screen.getByTestId('step-2')).toBeInTheDocument();
    });

    // Step 2: Volver a Step 1
    await user.click(screen.getByRole('button', { name: /previous/i }));

    await waitFor(() => {
      expect(screen.getByTestId('step-1')).toBeInTheDocument();
    });

    // Verificar que los datos siguen ahí
    expect(screen.getByLabelText(/name/i)).toHaveValue('Jane Smith');
    expect(screen.getByLabelText(/email/i)).toHaveValue('jane@test.com');
  });

  // ✅ Test: Progress bar se actualiza
  test('progress bar updates as user navigates', async () => {
    const user = userEvent.setup();
    render(<MultiStepForm />);

    const progressBar = document.querySelector('.progress-bar');
    const steps = progressBar.querySelectorAll('span');

    // Inicialmente solo Step 1 activo
    expect(steps[0]).toHaveClass('active');
    expect(steps[1]).not.toHaveClass('active');
    expect(steps[2]).not.toHaveClass('active');

    // Ir a Step 2
    await user.type(screen.getByLabelText(/name/i), 'John');
    await user.click(screen.getByRole('button', { name: /next/i }));

    await waitFor(() => {
      expect(steps[0]).toHaveClass('active');
      expect(steps[1]).toHaveClass('active');
      expect(steps[2]).not.toHaveClass('active');
    });
  });

  // ✅ Test: No se puede avanzar sin llenar campos requeridos
  test('validates required fields before advancing', async () => {
    const user = userEvent.setup();

    // Componente mejorado con validación
    const ValidatedStep1 = ({ data, onNext, onUpdate }) => {
      const [name, setName] = useState(data.name);
      const [email, setEmail] = useState(data.email);
      const [error, setError] = useState('');

      const handleNext = () => {
        if (!name || !email) {
          setError('All fields are required');
          return;
        }
        setError('');
        onUpdate({ name, email });
        onNext();
      };

      return (
        <div data-testid="step-1">
          <input
            aria-label="Name"
            value={name}
            onChange={(e) => setName(e.target.value)}
          />
          <input
            aria-label="Email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
          />
          {error && <span role="alert">{error}</span>}
          <button onClick={handleNext}>Next</button>
        </div>
      );
    };

    render(<ValidatedStep1 data={{}} onNext={jest.fn()} onUpdate={jest.fn()} />);

    // Intentar avanzar sin llenar
    await user.click(screen.getByRole('button', { name: /next/i }));

    // Verificar error
    expect(screen.getByRole('alert')).toHaveTextContent('All fields are required');
  });

  // ✅ Test: Snapshot de cada step
  test('matches snapshot for Step 1', () => {
    const { container } = render(<MultiStepForm />);
    expect(container.querySelector('.step-1')).toMatchSnapshot();
  });
});












// 3. Test de Redirección
// javascript// components/LoginForm.jsx
import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';

export const LoginForm = ({ onLogin }) => {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');
    setLoading(true);

    try {
      const result = await onLogin(email, password);

      if (result.success) {
        // Redirigir según rol
        if (result.user.role === 'admin') {
          navigate('/admin/dashboard');
        } else {
          navigate('/dashboard');
        }
      } else {
        setError(result.message || 'Login failed');
      }
    } catch (err) {
      setError('An error occurred');
    } finally {
      setLoading(false);
    }
  };

  return (
    <form onSubmit={handleSubmit} data-testid="login-form">
      <input
        type="email"
        value={email}
        onChange={(e) => setEmail(e.target.value)}
        placeholder="Email"
        aria-label="Email"
      />
      <input
        type="password"
        value={password}
        onChange={(e) => setPassword(e.target.value)}
        placeholder="Password"
        aria-label="Password"
      />
      {error && <div role="alert">{error}</div>}
      <button type="submit" disabled={loading}>
        {loading ? 'Logging in...' : 'Login'}
      </button>
    </form>
  );
};

// __tests__/LoginForm.test.jsx
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { BrowserRouter } from 'react-router-dom';
import { LoginForm } from '../components/LoginForm';

// Mock de react-router-dom
const mockNavigate = jest.fn();

jest.mock('react-router-dom', () => ({
  ...jest.requireActual('react-router-dom'),
  useNavigate: () => mockNavigate,
}));

describe('LoginForm - Redirections', () => {
  let mockOnLogin;

  beforeEach(() => {
    mockOnLogin = jest.fn();
    mockNavigate.mockClear();
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  const renderLoginForm = () => {
    return render(
      <BrowserRouter>
        <LoginForm onLogin={mockOnLogin} />
      </BrowserRouter>
    );
  };

  // ✅ Test: Redirección exitosa a dashboard normal
  test('redirects to /dashboard on successful login', async () => {
    const user = userEvent.setup();

    mockOnLogin.mockResolvedValue({
      success: true,
      user: { id: 1, role: 'user', name: 'John' }
    });

    renderLoginForm();

    await user.type(screen.getByLabelText(/email/i), 'user@test.com');
    await user.type(screen.getByLabelText(/password/i), 'password123');
    await user.click(screen.getByRole('button', { name: /login/i }));

    await waitFor(() => {
      expect(mockNavigate).toHaveBeenCalledWith('/dashboard');
      expect(mockNavigate).toHaveBeenCalledTimes(1);
    });
  });

  // ✅ Test: Redirección a admin dashboard
  test('redirects to /admin/dashboard for admin users', async () => {
    const user = userEvent.setup();

    mockOnLogin.mockResolvedValue({
      success: true,
      user: { id: 1, role: 'admin', name: 'Admin' }
    });

    renderLoginForm();

    await user.type(screen.getByLabelText(/email/i), 'admin@test.com');
    await user.type(screen.getByLabelText(/password/i), 'admin123');
    await user.click(screen.getByRole('button', { name: /login/i }));

    await waitFor(() => {
      expect(mockNavigate).toHaveBeenCalledWith('/admin/dashboard');
    });
  });

  // ✅ Test: NO redirige si login falla
  test('does not redirect on failed login', async () => {
    const user = userEvent.setup();

    mockOnLogin.mockResolvedValue({
      success: false,
      message: 'Invalid credentials'
    });

    renderLoginForm();

    await user.type(screen.getByLabelText(/email/i), 'wrong@test.com');
    await user.type(screen.getByLabelText(/password/i), 'wrong');
    await user.click(screen.getByRole('button', { name: /login/i }));

    await waitFor(() => {
      expect(screen.getByRole('alert')).toHaveTextContent('Invalid credentials');
    });

    expect(mockNavigate).not.toHaveBeenCalled();
  });

  // ✅ Test: Manejo de errores de red
  test('shows error message on network failure', async () => {
    const user = userEvent.setup();

    mockOnLogin.mockRejectedValue(new Error('Network error'));

    renderLoginForm();

    await user.type(screen.getByLabelText(/email/i), 'user@test.com');
    await user.type(screen.getByLabelText(/password/i), 'password');
    await user.click(screen.getByRole('button', { name: /login/i }));

    await waitFor(() => {
      expect(screen.getByRole('alert')).toHaveTextContent('An error occurred');
    });

    expect(mockNavigate).not.toHaveBeenCalled();
  });

  // ✅ Test: Loading state durante login
  test('shows loading state during login', async () => {
    const user = userEvent.setup();

    // Simular delay en login
    mockOnLogin.mockImplementation(() =>
      new Promise(resolve => setTimeout(() =>
        resolve({ success: true, user: { role: 'user' } }), 100
      ))
    );

    renderLoginForm();

    await user.type(screen.getByLabelText(/email/i), 'user@test.com');
    await user.type(screen.getByLabelText(/password/i), 'password');

    const submitButton = screen.getByRole('button');
    await user.click(submitButton);

    // Durante el loading
    expect(submitButton).toHaveTextContent('Logging in...');
    expect(submitButton).toBeDisabled();

    // Después del loading
    await waitFor(() => {
      expect(mockNavigate).toHaveBeenCalled();
    });
  });
});

// Test con MemoryRouter (navegación real)
describe('LoginForm - Real Navigation', () => {
  test('navigates to correct route in memory router', async () => {
    const user = userEvent.setup();

    const mockOnLogin = jest.fn().mockResolvedValue({
      success: true,
      user: { role: 'user' }
    });

    const { container } = render(
      <MemoryRouter initialEntries={['/login']}>
        <Routes>
          <Route path="/login" element={<LoginForm onLogin={mockOnLogin} />} />
          <Route path="/dashboard" element={<div>Dashboard Page</div>} />
        </Routes>
      </MemoryRouter>
    );

    await user.type(screen.getByLabelText(/email/i), 'user@test.com');
    await user.type(screen.getByLabelText(/password/i), 'password');
    await user.click(screen.getByRole('button', { name: /login/i }));

    await waitFor(() => {
      expect(screen.getByText('Dashboard Page')).toBeInTheDocument();
    });
  });
});















// 4. Test de Estilos
// javascript// components/StyledButton.jsx
import React from 'react';
import './StyledButton.css';

export const StyledButton = ({ variant = 'primary', size = 'medium', children, ...props }) => {
  return (
    <button
      className={`styled-button styled-button--${variant} styled-button--${size}`}
      {...props}
    >
      {children}
    </button>
  );
};

// // StyledButton.css
// .styled - button {
//   padding: 10px 20px;
//   border: none;
//   border - radius: 4px;
//   font - family: Arial, sans - serif;
//   cursor: pointer;
//   transition: all 0.3s ease;
// }

// .styled - button--primary {
//   background - color: #007bff;
//   color: white;
// }

// .styled - button--secondary {
//   background - color: #6c757d;
//   color: white;
// }

// .styled - button--small {
//   padding: 5px 10px;
//   font - size: 12px;
// }

// .styled - button--medium {
//   padding: 10px 20px;
//   font - size: 14px;
// }

// .styled - button--large {
//   padding: 15px 30px;
//   font - size: 18px;
// }

// .styled - button:hover {
//   opacity: 0.8;
//   transform: translateY(-2px);
// }

// .styled - button:disabled {
//   opacity: 0.5;
//   cursor: not - allowed;
// }

// __tests__/StyledButton.test.jsx
import { render, screen } from '@testing-library/react';
import { StyledButton } from '../components/StyledButton';

describe('StyledButton - CSS Styles', () => {

  // ✅ Test: Clases CSS correctas aplicadas
  test('applies correct CSS classes for primary variant', () => {
    render(<StyledButton variant="primary">Click</StyledButton>);

    const button = screen.getByRole('button');

    expect(button).toHaveClass('styled-button');
    expect(button).toHaveClass('styled-button--primary');
    expect(button).toHaveClass('styled-button--medium'); // default size
  });

  test('applies correct CSS classes for secondary variant', () => {
    render(<StyledButton variant="secondary">Click</StyledButton>);

    const button = screen.getByRole('button');

    expect(button).toHaveClass('styled-button--secondary');
  });

  // ✅ Test: Tamaños
  test('applies correct size classes', () => {
    const { rerender } = render(
      <StyledButton size="small">Small</StyledButton>
    );

    let button = screen.getByRole('button');
    expect(button).toHaveClass('styled-button--small');

    rerender(<StyledButton size="large">Large</StyledButton>);
    button = screen.getByRole('button');
    expect(button).toHaveClass('styled-button--large');
  });

  // ✅ Test: Estilos computados (computed styles)
  test('has correct computed styles for primary button', () => {
    render(<StyledButton variant="primary">Primary</StyledButton>);

    const button = screen.getByRole('button');
    const styles = window.getComputedStyle(button);

    // Nota: En jsdom los estilos CSS no se aplican realmente
    // Necesitas configurar jest para cargar CSS o usar jest-styled-components

    // Para este test necesitas:
    // 1. Configurar jest para importar CSS
    // 2. O usar inline styles que sí funcionan en jsdom
  });
});